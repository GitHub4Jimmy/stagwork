package com.stagwelltech.siren;

import com.stagwelltech.siren.exception.ConfigMissingException;
import com.stagwelltech.siren.exception.InitializationException;
import com.stagwelltech.siren.exception.ObjectMergeFailed;
import com.stagwelltech.siren.exception.ProviderNotFoundException;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;

public class SirenProviderTest extends BaseSirenTest {

    private static final Logger log = LoggerFactory.getLogger(SirenProviderTest.class);

    @Test
    @DisplayName("Create Siren instance and test the provider is set properly")
    public void createSiren() throws ConfigMissingException, IOException, ProviderNotFoundException, ObjectMergeFailed, InitializationException {
        log.info("starting test");
        Siren s = Siren.getInstance("test.siren.config.yaml");
        Assertions.assertEquals("com.stagwelltech.siren.sdk.impl.SirenSDKCWImpl", s.getSDK().getClass().getName());
        log.info("end test");
    }

    @Test
    @DisplayName("Invalid provider implementation class name")
    public void testInvalidProvider() {
        Assertions.assertThrows(ProviderNotFoundException.class, () -> {
            Siren s = Siren.getInstance("test.badprovider.yaml");
        });
    }

    @Test
    @DisplayName("Provider with no default constructor")
    public void testInvalidProvider2() {
        Assertions.assertThrows(ProviderNotFoundException.class, () -> {
            Siren s = Siren.getInstance("test.provider.nodefaultconstructor.yaml");
        });
    }

    @Test
    @DisplayName("Provider with no public constructor")
    public void testInvalidProvider3() {
        Assertions.assertThrows(ProviderNotFoundException.class, () -> {
            Siren s = Siren.getInstance("test.provider.nopublicconstructor.yaml");
        });
    }

    @Test
    @DisplayName("Provider with constructor exception")
    public void testInvalidProvider4() {
        Assertions.assertThrows(ProviderNotFoundException.class, () -> {
            Siren s = Siren.getInstance("test.provider.invocation.yaml");
        });
    }

    @Test
    @DisplayName("Provider is abstract class")
    public void testInvalidProvider5() {
        Assertions.assertThrows(ProviderNotFoundException.class, () -> {
            Siren s = Siren.getInstance("test.provider.instantiation.yaml");
        });
    }
}
