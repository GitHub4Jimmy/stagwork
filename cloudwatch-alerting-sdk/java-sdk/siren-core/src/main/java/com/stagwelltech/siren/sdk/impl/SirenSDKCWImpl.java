package com.stagwelltech.siren.sdk.impl;

import com.google.common.collect.Lists;
import com.google.common.collect.Maps;
import com.stagwelltech.siren.cloudwatch.CloudWatchClientFactory;
import com.stagwelltech.siren.config.SirenConfig;
import com.stagwelltech.siren.exception.InitializationException;
import com.stagwelltech.siren.exception.MetricException;
import com.stagwelltech.siren.exception.MissingCrentialsException;
import com.stagwelltech.siren.model.*;
import com.stagwelltech.siren.model.Metric;
import com.stagwelltech.siren.sdk.Pageable;
import com.stagwelltech.siren.sdk.SirenSDK;
import lombok.var;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import software.amazon.awssdk.services.cloudwatch.CloudWatchAsyncClient;
import software.amazon.awssdk.services.cloudwatch.model.*;

import java.net.URISyntaxException;
import java.time.Instant;
import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.Future;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;
import java.util.stream.Collectors;

public class SirenSDKCWImpl implements SirenSDK {

    private static final Logger log = LoggerFactory.getLogger(SirenSDKCWImpl.class);

    public static final String THREAD_RESPONSE_HANDLER_NAME = "ThreadResponseHandlerCW";

    private SirenConfig config;

    private CloudWatchAsyncClient cw;

    private ResponseHandlerThread responseThread;

    private Long syncProcessedCount = 0L;
    private Long syncErrorCount = 0L;

    private final Map<Integer, String> pageTokens = Maps.newConcurrentMap();

    private static final Long GET_TIMEOUT = 10000L;

    public SirenSDKCWImpl() {}

    @Override
    public void setConfig(SirenConfig config) {
        if (config == null) {
            throw new NullPointerException("Config is null");
        }
        this.config = config;
    }

    @Override
    public void initialize() throws InitializationException {
        try {
            this.cw = CloudWatchClientFactory.createClient(config);
        } catch (MissingCrentialsException | URISyntaxException e) {
            throw new InitializationException("Failed to initialize cloudwatch client", e);
        }

        if (config.async) {
            responseThread = new ResponseHandlerThread(config);
            responseThread.start();

            Runtime.getRuntime().addShutdownHook(new Thread(() -> {
                try {
                    responseThread.shutdown();
                    responseThread.interrupt();
                    responseThread.join(10000);
                } catch (InterruptedException e) {
                }
            }));
        }
    }

    public ResponseHandlerThread getResponseHandler() {
        return responseThread;
    }

    private List<Dimension> getDimensions(Map<String, String> tags) {
        List<Dimension> dimensions = Lists.newLinkedList();
        if (tags != null) {
            tags.forEach((s, s2) -> dimensions.add(
                    Dimension.builder().name(s).value(s2).build())
            );
        }
        return dimensions;
    }

    private Statistic getStatistic(AlarmAggregateType type) throws MetricException {
        switch (type) {
            case MIN:
                return Statistic.MINIMUM;
            case MAX:
                return Statistic.MAXIMUM;
            case SUM:
                return Statistic.SUM;
            case COUNT:
                return Statistic.SAMPLE_COUNT;
            case AVG:
                return Statistic.AVERAGE;
        }
        throw new MetricException("Invalid Statistics Aggregator");
    }

    private AlarmAggregateType getStatistic(Statistic type) {
        switch (type) {
            case AVERAGE:
                return AlarmAggregateType.AVG;
            case MAXIMUM:
                return AlarmAggregateType.MAX;
            case MINIMUM:
                return AlarmAggregateType.MIN;
            case SUM:
                return AlarmAggregateType.SUM;
            case SAMPLE_COUNT:
                return AlarmAggregateType.COUNT;
        }
        log.error("Could not resolve statistic type for {}", type);
        return null;
    }

    private ComparisonOperator getComparison(Comparison comp) throws MetricException {
        switch (comp) {
            case LT:
                return ComparisonOperator.LESS_THAN_THRESHOLD;
            case LTE:
                return ComparisonOperator.LESS_THAN_OR_EQUAL_TO_THRESHOLD;
            case GT:
                return ComparisonOperator.GREATER_THAN_THRESHOLD;
            case GTE:
                return ComparisonOperator.GREATER_THAN_OR_EQUAL_TO_THRESHOLD;
        }
        throw new MetricException("Invalid Comparison Operator");
    }

    private Comparison getComparison(ComparisonOperator comp) {
        switch (comp) {
            case LESS_THAN_LOWER_THRESHOLD:
                return Comparison.LT;
            case LESS_THAN_OR_EQUAL_TO_THRESHOLD:
                return Comparison.LTE;
            case GREATER_THAN_THRESHOLD:
                return Comparison.GT;
            case GREATER_THAN_OR_EQUAL_TO_THRESHOLD:
                return Comparison.GTE;
        }
        log.error("Could not resolve comparison type for {}", comp);
        return null;
    }

    private String getMissingData(MissingData md) {
        switch (md) {
            case BAD:
                return "breaching";
            case GOOD:
                return "notBreaching";
            default:
                return "ignore";
        }
    }

    private MissingData getMissingData(String md) {
        if (md != null) {
            if (md.equals("breaching")) {
                return MissingData.BAD;
            }
            if (md.equals("notBreaching")) {
                return MissingData.GOOD;
            }
            if (md.equals("ignore")) {
                return MissingData.IGNORE;
            }
        }
        log.error("Could not resolve missing data type for {}", md);
        return null;
    }

    private void handleResponse(Future<? extends CloudWatchResponse> future) throws MetricException, ExecutionException, InterruptedException {
        if (config.async) {
            responseThread.registerFuture(future);
        } else {
            CloudWatchResponse resp = future.get();
            if (!resp.sdkHttpResponse().isSuccessful()) {
                syncErrorCount++;
                log.error("HTTP {} {}", resp.sdkHttpResponse().statusCode(), resp.sdkHttpResponse().statusText());
                throw new MetricException(resp.sdkHttpResponse().statusText().orElse("Unknown Error"));
            } else {
                syncProcessedCount++;
            }
        }
    }

    @Override
    public void registerAlarm(AlarmDefinition def) throws MetricException {
        try {
            PutMetricAlarmRequest.Builder builder = PutMetricAlarmRequest.builder();

            List<Dimension> dimensions = getDimensions(def.getTags());

            builder.alarmName(def.getName())
                    .namespace(def.getCategory())
                    .alarmDescription(def.getDescription())
                    .metricName(def.getMetricName())
                    .dimensions(dimensions)
                    .datapointsToAlarm(def.getSpec().getNumberOfPointsToAlarm())
                    .evaluationPeriods(def.getSpec().getNumberOfPointsToCheck())
                    .period(def.getSpec().getTimeBetweenDataPoints())
                    .statistic(getStatistic(def.getSpec().getAggregation()))
                    .unit(StandardUnit.fromValue(def.getSpec().getAlarmUnit()))
                    .comparisonOperator(getComparison(def.getSpec().getThresholdComparison()))
                    .threshold(def.getSpec().getAlarmThreshold())
                    .treatMissingData(getMissingData(def.getSpec().getMissingData()));

            Future<PutMetricAlarmResponse> future = cw.putMetricAlarm(builder.build());
            handleResponse(future);

        } catch (Throwable t) {
            throw new MetricException("Error submitting alarm", t);
        }
    }

    @Override
    public Pageable<AlarmDefinition> getAlarms(int pageNumber, int recordCount) throws MetricException {
        try {
            DescribeAlarmsRequest.Builder builder = DescribeAlarmsRequest.builder();
            builder.maxRecords(recordCount);

            if (pageTokens.containsKey(pageNumber)) {
                builder.nextToken(pageTokens.get(pageNumber));
            } else {
                pageNumber = 0;
            }

            Future<DescribeAlarmsResponse> future = cw.describeAlarms(builder.build());
            DescribeAlarmsResponse response = future.get(GET_TIMEOUT, TimeUnit.MILLISECONDS);

            List<AlarmDefinition> definitions = Lists.newLinkedList();
            response.metricAlarms().forEach(a -> {
                var def = AlarmDefinition.builder();

                def.name(a.alarmName())
                   .metricName(a.metricName())
                   .description(a.alarmDescription())
                   .category(a.namespace())
                   .tags(a.dimensions().stream().collect(Collectors.toMap(Dimension::name, Dimension::value)))
                   .spec(
                           AlarmSpec.builder()
                           .numberOfPointsToAlarm(a.datapointsToAlarm())
                           .numberOfPointsToCheck(a.evaluationPeriods())
                           .timeBetweenDataPoints(a.period())
                           .aggregation(getStatistic(a.statistic()))
                           .alarmUnit(a.unit() != null ? a.unit().name() : null)
                           .thresholdComparison(getComparison(a.comparisonOperator()))
                           .missingData(getMissingData(a.treatMissingData()))
                           .alarmThreshold(a.threshold())
                           .build()
                   );

                definitions.add(def.build());
            });

            if (response.nextToken() != null) {
                pageTokens.put(pageNumber+1, response.nextToken());
            }

            return new CWPageable(definitions, pageNumber);

        } catch (TimeoutException | InterruptedException | ExecutionException e) {
            log.error("Timed out waiting for response", e);
            throw new MetricException(e);
        }
    }

    @Override
    public void deleteAlarm(String alarmName) throws MetricException {
        try {
            DeleteAlarmsRequest.Builder builder = DeleteAlarmsRequest.builder();
            builder.alarmNames(alarmName);

            Future<DeleteAlarmsResponse> future = cw.deleteAlarms(builder.build());
            handleResponse(future);

        } catch (Throwable t) {
            throw new MetricException("Error submitting alarm", t);
        }
    }

    @Override
    public void log(Metric metric) throws MetricException {
        try {
            PutMetricDataRequest.Builder request = PutMetricDataRequest.builder();

            List<Dimension> dimensions = getDimensions(metric.getTags());

            MetricDatum.Builder datum = MetricDatum.builder();
            datum.metricName(metric.getName());
            datum.timestamp(metric.getTs().toInstant());
            datum.counts(metric.getValue());
            datum.dimensions(dimensions);

            request.namespace(metric.getCategory());
            request.metricData(datum.build());

            Future<PutMetricDataResponse> future = cw.putMetricData(request.build());
            handleResponse(future);

        } catch (Throwable t) {
            throw new MetricException("Error submitting metric", t);
        }
    }

    @Override
    public SDKStats getSDKStats() {
        if (config.async) {
            return new SDKStats(responseThread.processedCount, responseThread.errorCount);
        } else {
            return new SDKStats(syncProcessedCount, syncErrorCount);
        }
    }

    public static class ResponseHandlerThread extends Thread {

        private static final Logger log = LoggerFactory.getLogger(ResponseHandlerThread.class);

        private final Object lock = new Object();

        private final List<Future<? extends CloudWatchResponse>> futuresList = new LinkedList<>();

        private Boolean exiting = false;

        private Long errorCount = 0L;

        private Long processedCount = 0L;

        private final SirenConfig config;

        public ResponseHandlerThread(SirenConfig config) {
            super(THREAD_RESPONSE_HANDLER_NAME);
            this.config = config;
        }

        public Long getErrorCount() {
            return errorCount;
        }

        public Long getProcessedCount() {
            return processedCount;
        }

        public void shutdown() {
            this.exiting = true;
        }

        public void registerFuture(Future<? extends CloudWatchResponse> future) {
            synchronized (lock) {
                futuresList.add(future);
            }
        }

        private static final Long TEST_EXECUTION_TIMEOUT = 50000L;
        /**
         * Used for testing purposes
         */
        public void waitForNextCompletion() throws InterruptedException {
            long startTime = Instant.now().toEpochMilli();
            do {
                synchronized (lock) {
                    if (processedCount > 0 || errorCount > 0) {
                        return;
                    }
                }
                Thread.sleep(100);
                long now = Instant.now().toEpochMilli();

                if (now - startTime > TEST_EXECUTION_TIMEOUT) {
                    throw new RuntimeException("Timeout waiting for test to complete");
                }
            } while (processedCount + errorCount < 1);
        }

        @Override
        public void run() {
            do {
                try {

                    synchronized (lock) {
                        Iterator<Future<? extends CloudWatchResponse>> iterator = futuresList.iterator();

                        while (iterator.hasNext()) {
                            Future<? extends CloudWatchResponse> f = iterator.next();
                            if (f.isDone()) {
                                iterator.remove();

                                CloudWatchResponse resp = f.get();

                                if (!resp.sdkHttpResponse().isSuccessful()) {
                                    errorCount++;
                                    log.error("HTTP {} {}", resp.sdkHttpResponse().statusCode(), resp.sdkHttpResponse().statusText());
                                } else {
                                    processedCount++;
                                }
                            }
                        }
                    }

                    Thread.sleep(config.asyncWaitTime);
                } catch (InterruptedException e) {
                    errorCount++;
                    if (errorCount < 10 || errorCount % 100 == 0) {
                        log.error("Response thread was interrupted [{}]", errorCount);
                    }
                } catch (ExecutionException e) {
                    errorCount++;
                    log.error("Exception executing cloudwatch request", e);
                }
            } while (!exiting);
        }
    }
}
