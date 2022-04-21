package com.stagwelltech.siren.spring.util;

import com.stagwelltech.siren.sdk.annotation.Monitored;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

@Service
public class MonitorMethods {

    private static final Logger log = LoggerFactory.getLogger(MonitorMethods.class);

    @Monitored(category = "UnitTests", metricName = "CustomshortMetric")
    public short fireLogshort() {
        return 11;
    }

    @Monitored(category = "UnitTests", metricName = "CustomShortMetric")
    public Short fireLogShort() {
        return 12;
    }

    @Monitored(category = "UnitTests", metricName = "CustomShortMetric")
    public int fireLogInt() {
        return 10;
    }

    @Monitored(category = "UnitTests", metricName = "CustomIntegerMetric")
    public Integer fireLogInteger() {
        return 15;
    }

    @Monitored(category = "UnitTests", metricName = "CustomlongMetric")
    public long fireLoglong() {
        return 13L;
    }

    @Monitored(category = "UnitTests", metricName = "CustomLongMetric")
    public Long fireLogLong() {
        return 14L;
    }

    @Monitored(category = "UnitTests", metricName = "CustomfloatMetric")
    public float fireLogfloat() {
        return 20.0f;
    }

    @Monitored(category = "UnitTests", metricName = "CustomFloatMetric")
    public Float fireLogFloat() {
        return 21.0f;
    }

    @Monitored(category = "UnitTests", metricName = "CustomdoubleMetric")
    public double fireLogdouble() {
        return 20.0;
    }

    @Monitored(category = "UnitTests", metricName = "CustomDoubleMetric")
    public Double fireLogDouble() {
        return 21.0;
    }

    @Monitored(category = "UnitTests", metricName = "CustombooleanMetric")
    public boolean fireLogboolean() {
        return true;
    }

    @Monitored(category = "UnitTests", metricName = "CustomBooleanMetric")
    public Boolean fireLogBoolean() {
        return false;
    }

    //will fail to fire metric
    @Monitored(category = "UnitTests", metricName = "CustomcharMetric")
    public char fireLogchar() {
        return 'a';
    }

    //will fail to fire metric
    @Monitored(category = "UnitTests", metricName = "CustomCharMetric")
    public Character fireLogChar() {
        return 'b';
    }

    //will fail to fire metric
    @Monitored(category = "UnitTests", metricName = "CustomStringMetric")
    public String fireLogString() {
        return "hello, world";
    }

    public static class TestObj {
        int x = 100;
        String s = "hello";
    }

    //will fail to fire metric
    @Monitored(category = "UnitTests", metricName = "CustomObjectMetric")
    public TestObj fireLogObject() {
        return new TestObj();
    }


    @Monitored(category = "UnitTests", metricName = "CustomIntegerMetric", alarmName = "CustomIntegerMetric", description = "Unit testing alert", alarmThreshold = 20)
    public Integer fireLogIntegerAlert() {
        return 15;
    }
}
