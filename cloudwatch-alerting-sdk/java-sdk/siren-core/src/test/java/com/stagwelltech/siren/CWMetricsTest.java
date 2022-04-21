package com.stagwelltech.siren;

import com.stagwelltech.siren.exception.*;
import com.stagwelltech.siren.model.*;
import com.stagwelltech.siren.sdk.Pageable;
import com.stagwelltech.siren.sdk.impl.SirenSDKCWImpl;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Disabled;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfEnvironmentVariable;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;
import java.util.Date;
import java.util.stream.Collectors;
import java.util.stream.Stream;

@EnabledIfEnvironmentVariable(named = "DOCKER_LOCATION", matches = ".*")
public class CWMetricsTest extends BaseAWSTest {

    private static final Logger log = LoggerFactory.getLogger(CWMetricsTest.class);

    @Test
    @DisplayName("Test firing a metric at CW api")
    public void testMetricA() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, MetricException, InterruptedException {

        Siren s = Siren.getInstance(tempConfigFileName);

        Metric.Builder m = Metric.builder();
        m.category("UnitTestMetric")
                .name("testMetricA")
                .ts(new Date())
                .value(101.1)
                .tags(Stream.of(new String[][] {{ "Tag1", "1" },{ "Tag2", "2" }}).collect(Collectors.toMap(data -> data[0], data -> data[1])));

        s.getSDK().log(m.build());

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertEquals(1, stats.getProcessedCount());
    }

    @Test
    @DisplayName("Test creating an alarm")
    public void testCreateAlarm() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, MetricException, InterruptedException {
        Siren s = Siren.getInstance(tempConfigFileName);

        AlarmDefinition.Builder builder = AlarmDefinition.builder();
        builder.name("Test Alarm")
                .category("UnitTestAlarm")
                .description("This is a test description")
                .tags(Stream.of(new String[][] {{ "Tag1", "1" },{ "Tag2", "2" }}).collect(Collectors.toMap(data -> data[0], data -> data[1])))
                .spec(AlarmSpec.builder()
                        .numberOfPointsToAlarm(1)
                        .numberOfPointsToCheck(1)
                        .timeBetweenDataPoints(60)
                        .aggregation(AlarmAggregateType.AVG)
                        .alarmUnit("Seconds")
                        .thresholdComparison(Comparison.GT)
                        .alarmThreshold(10.0)
                        .missingData(MissingData.IGNORE)
                        .build()
                );

        s.getSDK().registerAlarm(builder.build());

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertEquals(1, stats.getProcessedCount());
    }

    @Test
    @DisplayName("Test listing alarms")
    @Disabled("Enable when localstack cloudwatch sdk date formats arent broken")
    public void testListAlarms() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, MetricException, InterruptedException {
        Siren s = Siren.getInstance(tempConfigFileName);

        Pageable<AlarmDefinition> definitions = s.getSDK().getAlarms(0, 100);
        log.info("got {} alarms", definitions.getPageCount());
    }

    @Test
    @DisplayName("Test deleting an alarm")
    @Disabled("Enable when localstack supports the delete alarm API")
    public void testDeleteAlarm() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, MetricException, InterruptedException {
        Siren s = Siren.getInstance(tempConfigFileName);

        s.getSDK().deleteAlarm("Test Alarm");

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertEquals(1, stats.getProcessedCount());
    }
}
