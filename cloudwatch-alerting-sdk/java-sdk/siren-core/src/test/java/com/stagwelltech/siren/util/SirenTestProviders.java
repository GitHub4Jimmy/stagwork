package com.stagwelltech.siren.util;

import com.stagwelltech.siren.config.SirenConfig;
import com.stagwelltech.siren.exception.InitializationException;
import com.stagwelltech.siren.exception.MetricException;
import com.stagwelltech.siren.model.AlarmDefinition;
import com.stagwelltech.siren.model.Metric;
import com.stagwelltech.siren.model.SDKStats;
import com.stagwelltech.siren.sdk.Pageable;
import com.stagwelltech.siren.sdk.SirenSDK;

public class SirenTestProviders {

    public static abstract class TestProviderBase implements SirenSDK {
        @Override
        public void initialize() throws InitializationException {}
        @Override
        public void setConfig(SirenConfig config) {}
        @Override
        public void registerAlarm(AlarmDefinition def) {}
        @Override
        public void deleteAlarm(String metricName) {}
        @Override
        public void log(Metric metric) {}
        @Override
        public SDKStats getSDKStats() {
            return null;
        }
        @Override
        public Pageable<AlarmDefinition> getAlarms(int pageNumber, int recordCount) throws MetricException {return null;}
    }

    public static class SirenProviderNoDefaultConstructor extends TestProviderBase {
        public SirenProviderNoDefaultConstructor(String input) {}
    }

    public static class SirenProviderPrivateConstructor extends TestProviderBase {
        private SirenProviderPrivateConstructor() {}
    }

    public static class SirenProviderExceptionConstructor extends TestProviderBase {
        public SirenProviderExceptionConstructor() throws Exception { throw new Exception("failed to instantiate");}
    }

    public static abstract class SirenProviderAbstract extends TestProviderBase {
        public SirenProviderAbstract() {}
    }

    public static class SirenProviderBasic extends TestProviderBase {
        public SirenProviderBasic() {}
    }
}
