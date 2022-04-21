package com.stagwelltech.siren.config;

import lombok.Data;

@Data
public class SirenConfig {
    public static class AwsConfig {
        public String endpoint;
        public String region;
        public String profile;
        public String accessKey;
        public String secretKey;
        public Integer httpConnectionsCount;
    }

    public boolean enabled = true;
    public String provider;
    public AwsConfig aws;
    public Boolean async = true;
    public Integer asyncWaitTime = 500;
}
