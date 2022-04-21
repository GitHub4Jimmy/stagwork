package com.stagwelltech.siren.model;

import lombok.Builder;
import lombok.Data;
import lombok.NonNull;

@Data
@Builder(builderClassName = "Builder")
public class AlarmSpec {

    /**
     * How many data points need to be outside our defined threshold in order to fire an alarm
     */
    private Integer numberOfPointsToAlarm;

    /**
     * How many points are we checking for alarm conditions. This is the number of points in the bucket, against which we check for numberOfPointsToAlarm points which breach our threshold.
     */
    private Integer numberOfPointsToCheck;

    /**
     * The time in seconds between data points.
     */
    private Integer timeBetweenDataPoints;

    /**
     * How to process the data between data points to create a single data point.
     */
    private AlarmAggregateType aggregation;

    /**
     * Provider specific string describing the units of measurement.
     * AWS CloudWatch Provider - Evaluates to StandardUnit.fromValue(alarmUnit) before sending to CW
     */
    private String alarmUnit;

    /**
     * How to test against the alarm threshold
     */
    private Comparison thresholdComparison;

    /**
     * The actual threshold value
     */
    private Double alarmThreshold;

    /**
     * How to handle missing data
     */
    private MissingData missingData;
}
