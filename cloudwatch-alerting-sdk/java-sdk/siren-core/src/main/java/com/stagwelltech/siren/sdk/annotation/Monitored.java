package com.stagwelltech.siren.sdk.annotation;

import com.stagwelltech.siren.model.AlarmAggregateType;
import com.stagwelltech.siren.model.Comparison;
import com.stagwelltech.siren.model.MissingData;
import com.stagwelltech.siren.sdk.metric.ValueResolver;
import com.stagwelltech.siren.sdk.metric.impl.ScalarTypeValueResolver;

import java.lang.annotation.*;

@Target({ElementType.METHOD})
@Retention(RetentionPolicy.RUNTIME)
@Inherited
public @interface Monitored {
    String metricName();
    String alarmName() default "";
    String category() default "Custom Metrics";
    String description() default "No description";
    int numberOfPointsToAlarm() default 1;
    int numberOfPointsToCheck() default 1;
    int timeBetweenDataPoints() default 60;
    AlarmAggregateType aggregation() default AlarmAggregateType.AVG;
    String alarmUnit() default "Seconds";
    Comparison thresholdComparison() default Comparison.GTE;
    double alarmThreshold() default 0;
    MissingData missingData() default MissingData.IGNORE;
    Class<? extends ValueResolver> valueResolver() default ScalarTypeValueResolver.class;
}
