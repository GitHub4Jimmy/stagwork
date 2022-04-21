package com.stagwelltech.siren;

import cloud.localstack.docker.LocalstackDockerExtension;
import cloud.localstack.docker.annotation.IEnvironmentVariableProvider;
import cloud.localstack.docker.annotation.IHostNameResolver;
import cloud.localstack.docker.annotation.LocalHostNameResolver;
import cloud.localstack.docker.annotation.LocalstackDockerProperties;
import com.google.common.collect.Maps;
import com.stagwelltech.siren.config.SirenConfig;
import com.stagwelltech.siren.util.YAML;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.extension.ExtendWith;

import java.io.File;
import java.io.IOException;
import java.util.Map;

@ExtendWith(LocalstackDockerExtension.class)
@LocalstackDockerProperties(services = {"cloudwatch"}, hostNameResolver= BaseAWSTest.DockerHostNameResolver.class, environmentVariableProvider = BaseAWSTest.DockerHostEnvProvider.class, pullNewImage = true)
public abstract class BaseAWSTest extends BaseSirenTest {

    public static class DockerHostNameResolver implements IHostNameResolver {
        private static final LocalHostNameResolver fallbackResolver = new LocalHostNameResolver();

        @Override
        public String getHostName() {
            String dockerHost = System.getenv("DOCKER_HOST");
            if (dockerHost == null) {
                return fallbackResolver.getHostName();
            }
            //todo: make sure this is the right format
            return dockerHost;
        }
    }

    public static class DockerHostEnvProvider implements IEnvironmentVariableProvider {
        @Override
        public Map<String, String> getEnvironmentVariables() {
            Map<String, String> vars = Maps.newHashMap();

            vars.put("EDGE_PORT", "4566");
            //vars.put("PORT_WEB_UI", "8081");
            vars.put("START_WEB", "0");

            return vars;
        }
    }

    public static class LocalStackEndpointResolver {
        public String getAwsEndpoint() {
            DockerHostNameResolver resolver = new DockerHostNameResolver();
            return "http://" + resolver.getHostName() + ":4566";
        }
    }

    public static String tempConfigFileName;

    @BeforeEach
    protected void updateClientConfig() throws IllegalAccessException, IOException {
        super.clearSirenInstance();
        setupTempConfigFile();
    }

    @Override
    protected void clearSirenInstance() throws IllegalAccessException {}

    public static final void setupTempConfigFile() throws IOException {
        File temp = File.createTempFile("temp", ".yaml");
        tempConfigFileName = temp.getAbsolutePath();

        SirenConfig config = YAML.LoadClassPath("test.siren.config.yaml", "siren", SirenConfig.class);
        LocalStackEndpointResolver resolver = new LocalStackEndpointResolver();
        config.aws.endpoint = resolver.getAwsEndpoint();

        YAML.WriteFile(tempConfigFileName, "siren", config);
    }

    @AfterEach
    protected void cleanupConfigFile() {
        new File(tempConfigFileName).delete();
    }

}
