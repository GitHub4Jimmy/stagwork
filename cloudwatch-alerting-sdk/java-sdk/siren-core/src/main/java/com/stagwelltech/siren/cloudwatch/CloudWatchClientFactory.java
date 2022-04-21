package com.stagwelltech.siren.cloudwatch;

import com.stagwelltech.siren.config.SirenConfig;
import com.stagwelltech.siren.exception.MissingCrentialsException;
import software.amazon.awssdk.auth.credentials.AwsBasicCredentials;
import software.amazon.awssdk.auth.credentials.ProfileCredentialsProvider;
import software.amazon.awssdk.auth.credentials.StaticCredentialsProvider;
import software.amazon.awssdk.http.nio.netty.NettyNioAsyncHttpClient;
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.cloudwatch.CloudWatchAsyncClient;
import software.amazon.awssdk.services.cloudwatch.CloudWatchAsyncClientBuilder;

import java.net.URI;
import java.net.URISyntaxException;

public class CloudWatchClientFactory {

    public static final synchronized CloudWatchAsyncClient createClient(SirenConfig config) throws MissingCrentialsException, URISyntaxException {

        CloudWatchAsyncClientBuilder builder = CloudWatchAsyncClient.builder();

        if (config.aws != null) {
            if (config.aws.region != null) {
                builder.region(Region.of(config.aws.region));
            }

            if (config.aws.profile != null) {
                builder.credentialsProvider(ProfileCredentialsProvider.create(config.aws.profile));
            }

            if (config.aws.accessKey != null) {
                if (config.aws.secretKey == null) {
                    throw new MissingCrentialsException("Missing aws secret key");
                }
                builder.credentialsProvider(StaticCredentialsProvider.create(AwsBasicCredentials.create(config.aws.accessKey, config.aws.secretKey)));
            }

            if (config.aws.httpConnectionsCount != null) {
                builder.httpClientBuilder(NettyNioAsyncHttpClient.builder()
                        .maxConcurrency(config.aws.httpConnectionsCount)
                        .maxPendingConnectionAcquires(10000));
            }

            if (config.aws.endpoint != null) {
                builder.endpointOverride(new URI(config.aws.endpoint));
            }
        }

        return builder.build();
    }
}
