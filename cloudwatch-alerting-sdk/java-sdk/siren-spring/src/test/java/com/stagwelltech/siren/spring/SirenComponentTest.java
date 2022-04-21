package com.stagwelltech.siren.spring;

import com.stagwelltech.siren.BaseAWSTest;
import com.stagwelltech.siren.Siren;
import com.stagwelltech.siren.exception.*;
import com.stagwelltech.siren.model.SDKStats;
import com.stagwelltech.siren.sdk.impl.SirenSDKCWImpl;
import com.stagwelltech.siren.spring.util.MonitorMethods;
import com.stagwelltech.siren.spring.util.SirenFactory;
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.condition.EnabledIfEnvironmentVariable;
import org.junit.jupiter.api.extension.ExtendWith;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.context.annotation.EnableAspectJAutoProxy;
import org.springframework.test.context.junit.jupiter.SpringExtension;

import java.io.IOException;

@ExtendWith(SpringExtension.class)
@SpringBootTest(classes = {MonitoringAdvice.class, MonitorMethods.class, SirenFactory.class, ComponentScanner.class})
@EnabledIfEnvironmentVariable(named = "DOCKER_LOCATION", matches = ".*")
@EnableAspectJAutoProxy
public class SirenComponentTest extends BaseAWSTest {

    private static final Logger log = LoggerFactory.getLogger(SirenComponentTest.class);

    @Autowired
    private final MonitorMethods monitor = null;

    @Autowired
    private final Siren s = null;

    @Autowired
    private final MonitoringAdvice monitorAdvice = null;

    @BeforeAll
    public static void initialize() throws IOException {
        setupTempConfigFile();
    }

    @Override
    @BeforeEach
    protected void updateClientConfig() throws IllegalAccessException, IOException {}

    @Test
    @DisplayName("Test firing a metric as an int")
    public void runTestInt() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogInt();

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertTrue(stats.getProcessedCount() > 0);
    }

    @Test
    @DisplayName("Test firing a metric as an Integer")
    public void runTestInteger() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogInteger();

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertTrue(stats.getProcessedCount() > 0);
    }

    @Test
    @DisplayName("Test firing a metric as an short")
    public void runTestshort() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogshort();

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertTrue(stats.getProcessedCount() > 0);
    }

    @Test
    @DisplayName("Test firing a metric as an Short")
    public void runTestShort() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogShort();

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertTrue(stats.getProcessedCount() > 0);
    }

    @Test
    @DisplayName("Test firing a metric as an long")
    public void runTestlong() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLoglong();

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertTrue(stats.getProcessedCount() > 0);
    }

    @Test
    @DisplayName("Test firing a metric as an Long")
    public void runTestLong() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogLong();

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertTrue(stats.getProcessedCount() > 0);
    }

    @Test
    @DisplayName("Test firing a metric as an float")
    public void runTestfloat() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogfloat();

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertTrue(stats.getProcessedCount() > 0);
    }

    @Test
    @DisplayName("Test firing a metric as an Float")
    public void runTestFloat() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogFloat();

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertTrue(stats.getProcessedCount() > 0);
    }

    @Test
    @DisplayName("Test firing a metric as an double")
    public void runTestdouble() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogdouble();

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertTrue(stats.getProcessedCount() > 0);
    }

    @Test
    @DisplayName("Test firing a metric as an Double")
    public void runTestDouble() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogDouble();

        SirenSDKCWImpl.ResponseHandlerThread responseThread = ((SirenSDKCWImpl)s.getSDK()).getResponseHandler();

        log.info("Waiting for request to complete");
        responseThread.waitForNextCompletion();

        SDKStats stats = s.getSDK().getSDKStats();
        Assertions.assertTrue(stats.getProcessedCount() > 0);
    }

    @Test
    @DisplayName("Test firing a metric as an char")
    public void runTestchar() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogchar();

        Throwable t = monitorAdvice.getLastException();
        Assertions.assertNotNull(t);
        Assertions.assertTrue(t instanceof MetricException);
    }

    @Test
    @DisplayName("Test firing a metric as an Character")
    public void runTestCharacter() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogChar();

        Throwable t = monitorAdvice.getLastException();
        Assertions.assertNotNull(t);
        Assertions.assertTrue(t instanceof MetricException);
    }

    @Test
    @DisplayName("Test firing a metric as an String")
    public void runTestString() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogString();

        Throwable t = monitorAdvice.getLastException();
        Assertions.assertNotNull(t);
        Assertions.assertTrue(t instanceof MetricException);
    }

    @Test
    @DisplayName("Test firing a metric as an Object")
    public void runTestObject() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {

        monitor.fireLogObject();

        Throwable t = monitorAdvice.getLastException();
        Assertions.assertNotNull(t);
        Assertions.assertTrue(t instanceof MetricException);
    }

    @Test
    @DisplayName("Test creating an alarm metric")
    public void runTestAlert1() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException, InterruptedException {
        monitor.fireLogIntegerAlert();
    }

}
