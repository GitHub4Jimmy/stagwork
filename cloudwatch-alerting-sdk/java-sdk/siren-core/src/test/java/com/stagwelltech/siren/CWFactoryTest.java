package com.stagwelltech.siren;

import com.stagwelltech.siren.cloudwatch.CloudWatchClientFactory;
import com.stagwelltech.siren.config.SirenConfig;
import com.stagwelltech.siren.exception.ConfigMissingException;
import com.stagwelltech.siren.exception.MissingCrentialsException;
import com.stagwelltech.siren.util.YAML;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import software.amazon.awssdk.core.exception.SdkClientException;
import software.amazon.awssdk.services.cloudwatch.CloudWatchAsyncClient;
import software.amazon.awssdk.services.cloudwatch.model.ListMetricsResponse;

import java.io.IOException;
import java.net.URISyntaxException;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.Future;

public class CWFactoryTest {

    private static final Logger log = LoggerFactory.getLogger(CWFactoryTest.class);

    @Test
    @DisplayName("Test using default credentials chain")
    public void testDefault() throws IOException, MissingCrentialsException, URISyntaxException {
        SirenConfig config = YAML.LoadClassPath("test.siren.config.yaml", "siren", SirenConfig.class);
        CloudWatchAsyncClient client = CloudWatchClientFactory.createClient(config);
        Assertions.assertNotNull(client);
    }

    @Test
    @DisplayName("Test using region specifier")
    public void testRegionConfig() throws IOException, MissingCrentialsException, URISyntaxException {
        SirenConfig config = YAML.LoadClassPath("test.siren.config.yaml", "siren", SirenConfig.class);
        config.aws = new SirenConfig.AwsConfig();
        config.aws.region = "us-east-1";
        CloudWatchAsyncClient client = CloudWatchClientFactory.createClient(config);
        Assertions.assertNotNull(client);
    }

    @Test
    @DisplayName("Test using invalid region specifier")
    public void testRegionConfigBad() throws IOException, MissingCrentialsException, ExecutionException, InterruptedException, URISyntaxException {
        SirenConfig config = YAML.LoadClassPath("test.siren.config.yaml", "siren", SirenConfig.class);
        config.aws = new SirenConfig.AwsConfig();
        config.aws.region = "badRegion";
        CloudWatchAsyncClient client = CloudWatchClientFactory.createClient(config);
        Assertions.assertNotNull(client);

        Assertions.assertThrows(ExecutionException.class, () -> {
            Future<ListMetricsResponse> future = client.listMetrics();
            ListMetricsResponse resp = future.get();
        });
    }

    @Test
    @DisplayName("Test using invalid profile specifier")
    public void testProfileBad() throws IOException, MissingCrentialsException, ExecutionException, InterruptedException, URISyntaxException {
        SirenConfig config = YAML.LoadClassPath("test.siren.config.yaml", "siren", SirenConfig.class);
        config.aws = new SirenConfig.AwsConfig();
        config.aws.profile = "badProfile";
        CloudWatchAsyncClient client = CloudWatchClientFactory.createClient(config);
        Assertions.assertNotNull(client);

        Assertions.assertThrows(ExecutionException.class, () -> {
            Future<ListMetricsResponse> future = client.listMetrics();
            ListMetricsResponse resp = future.get();
        });
    }

    @Test
    @DisplayName("Test using invalid static credentials")
    public void testCredsBad() throws IOException, MissingCrentialsException, ExecutionException, InterruptedException, URISyntaxException {
        SirenConfig config = YAML.LoadClassPath("test.siren.config.yaml", "siren", SirenConfig.class);
        config.aws = new SirenConfig.AwsConfig();
        config.aws.accessKey = "badKey";
        config.aws.secretKey = "badSecret";
        CloudWatchAsyncClient client = CloudWatchClientFactory.createClient(config);
        Assertions.assertNotNull(client);

        Assertions.assertThrows(ExecutionException.class, () -> {
            Future<ListMetricsResponse> future = client.listMetrics();
            ListMetricsResponse resp = future.get();
        });
    }

    @Test
    @DisplayName("Test setting http concurrency")
    public void testConcurrency() throws IOException, MissingCrentialsException, ExecutionException, InterruptedException, URISyntaxException {
        SirenConfig config = YAML.LoadClassPath("test.siren.config.yaml", "siren", SirenConfig.class);
        config.aws = new SirenConfig.AwsConfig();
        config.aws.httpConnectionsCount = 150;
        CloudWatchAsyncClient client = CloudWatchClientFactory.createClient(config);
        Assertions.assertNotNull(client);
    }

}
