package com.stagwelltech.siren.spring;

import com.stagwelltech.siren.Siren;
import com.stagwelltech.siren.exception.MetricException;
import com.stagwelltech.siren.model.*;
import com.stagwelltech.siren.sdk.annotation.Monitored;
import com.stagwelltech.siren.spring.util.AnnotationScanner;
import org.apache.commons.validator.routines.DoubleValidator;
import org.apache.commons.validator.routines.IntegerValidator;
import org.apache.commons.validator.routines.RegexValidator;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.BeansException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.ApplicationContext;
import org.springframework.context.ApplicationContextAware;
import org.springframework.context.annotation.Configuration;

import javax.annotation.PostConstruct;
import java.util.Date;
import java.util.Set;
import java.util.stream.Collectors;
import java.util.stream.Stream;

@Configuration
public class ComponentScanner implements ApplicationContextAware {

    private static final Logger log = LoggerFactory.getLogger(ComponentScanner.class);

    private ApplicationContext context = null;

    private final DoubleValidator dblV = DoubleValidator.getInstance();

    private final IntegerValidator intV = IntegerValidator.getInstance();

    private final RegexValidator rgV = new RegexValidator(".+");

    @Autowired
    private final Siren siren = null;

    @Override
    public void setApplicationContext(ApplicationContext applicationContext) throws BeansException {
        this.context = applicationContext;
    }

    @PostConstruct
    public void initializeAllAlarms() throws Exception {

        if (siren.isEnabled()) {

            Set<AnnotationScanner.AnnotatedMethod> methods = AnnotationScanner.FindAnnotatedMethods(context, Monitored.class);

            methods.forEach(am -> {

                Monitored m = am.method.getAnnotation(Monitored.class);
                if (!m.alarmName().isEmpty()) {
                    if (!validateAlarmDef(m)) {
                        log.error("Alarm definition [{}] did not pass validation", m.alarmName());
                        return;
                    }

                    log.info("Creating alarm definition for [{}]", m.alarmName());

                    AlarmDefinition.Builder builder = AlarmDefinition.builder();
                    builder.name(m.alarmName())
                            .category(m.category())
                            .description(m.description())
                            .tags(Stream.of(new String[][]{{"CreatedBy", "StagwellTech SIREN Alerts"}, {"CreatedOn", new Date().toString()}}).collect(Collectors.toMap(data -> data[0], data -> data[1])))
                            .spec(AlarmSpec.builder()
                                    .numberOfPointsToAlarm(m.numberOfPointsToAlarm())
                                    .numberOfPointsToCheck(m.numberOfPointsToCheck())
                                    .timeBetweenDataPoints(m.timeBetweenDataPoints())
                                    .aggregation(m.aggregation())
                                    .alarmUnit(m.alarmUnit())
                                    .thresholdComparison(m.thresholdComparison())
                                    .alarmThreshold(m.alarmThreshold())
                                    .missingData(m.missingData())
                                    .build()
                            );

                    try {
                        siren.getSDK().registerAlarm(builder.build());
                    } catch (MetricException e) {
                        log.error("UNable to register alarm [{}]", m.alarmName(), e);
                    }
                }
            });
        }
    }

    private boolean validateAlarmDef(Monitored m) {
        if (!rgV.isValid(m.alarmName())) {
            return false;
        }
        if (!rgV.isValid(m.description())) {
            return false;
        }
        if (!rgV.isValid(m.category())) {
            return false;
        }
        if (!rgV.isValid(m.metricName())) {
            return false;
        }
        if (!rgV.isValid(m.alarmUnit())) {
            return false;
        }
        if (!intV.minValue(m.numberOfPointsToAlarm(), 1)) {
            return false;
        }
        if (!intV.minValue(m.numberOfPointsToCheck(), 1)) {
            return false;
        }
        if (!intV.minValue(m.numberOfPointsToCheck(), m.numberOfPointsToAlarm())) {
            return false;
        }
        if (!intV.minValue(m.timeBetweenDataPoints(), 1)) {
            return false;
        }

        return true;
    }
}
