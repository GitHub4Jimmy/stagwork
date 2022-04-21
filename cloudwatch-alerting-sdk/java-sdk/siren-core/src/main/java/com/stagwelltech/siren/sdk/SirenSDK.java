package com.stagwelltech.siren.sdk;

import com.stagwelltech.siren.config.SirenConfig;
import com.stagwelltech.siren.exception.InitializationException;
import com.stagwelltech.siren.exception.MetricException;
import com.stagwelltech.siren.model.AlarmDefinition;
import com.stagwelltech.siren.model.Metric;
import com.stagwelltech.siren.model.SDKStats;

import java.util.List;

public interface SirenSDK {
    void setConfig(SirenConfig config);
    void initialize() throws InitializationException;
    void registerAlarm(AlarmDefinition def) throws MetricException;
    void deleteAlarm(String alarmName) throws MetricException;
    Pageable<AlarmDefinition> getAlarms(int pageNumber, int recordCount) throws MetricException;
    void log(Metric metric) throws MetricException;
    SDKStats getSDKStats();
}
