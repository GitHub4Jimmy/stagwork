package com.stagwelltech.siren;

import com.stagwelltech.siren.config.SirenConfig;
import com.stagwelltech.siren.exception.ConfigMissingException;
import com.stagwelltech.siren.exception.InitializationException;
import com.stagwelltech.siren.exception.ObjectMergeFailed;
import com.stagwelltech.siren.exception.ProviderNotFoundException;
import com.stagwelltech.siren.util.BeanUtils;
import com.stagwelltech.siren.util.YAML;
import com.stagwelltech.siren.util.YAMLTestModel;
import org.junit.jupiter.api.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;

public class SirenConfigTest extends BaseSirenTest {

    private static final Logger log = LoggerFactory.getLogger(SirenConfigTest.class);

    @Test
    @DisplayName("Load sirten config file and verify the structure")
    public void testConfig() throws IOException {
        SirenConfig config = YAML.LoadClassPath("test.siren.config.yaml", "siren", SirenConfig.class);
        Assertions.assertEquals("com.stagwelltech.siren.sdk.impl.SirenSDKCWImpl", config.provider);
    }

    @Test
    @DisplayName("Create an instance of siren and verify the config")
    public void createSiren() throws IOException, ConfigMissingException, ProviderNotFoundException, ObjectMergeFailed, InitializationException {
        Siren s = Siren.getInstance();
        Assertions.assertEquals("com.stagwelltech.siren.sdk.impl.SirenSDKCWImpl", s.getProviderClassName());
    }

    @Test
    @DisplayName("Missing config file")
    public void testMissingConfig() {
        Assertions.assertThrows(ConfigMissingException.class, () -> {
            Siren s = Siren.getInstance("test.missing.yaml");
        });
    }

    @Test
    @DisplayName("Object Merge")
    public void testMerge() throws IOException, ObjectMergeFailed {
        YAMLTestModel config = YAML.LoadClassPath("test.basename.yaml", "test", YAMLTestModel.class);
        YAMLTestModel configOverride = YAML.LoadClassPath("test.basename.override.yaml", "test", YAMLTestModel.class);

        BeanUtils.merge(config, configOverride);

        Assertions.assertEquals(12345, config.i);
        Assertions.assertEquals(12.5, config.f, 0.001f);
        Assertions.assertEquals(false, config.b);
        Assertions.assertEquals("override", config.s);
    }

    @Test
    @DisplayName("Test config overrides")
    public void configOverride() throws IOException, ConfigMissingException, ProviderNotFoundException, ObjectMergeFailed, InitializationException {
        try {
            String path = getClass().getClassLoader().getResource("test.siren.config.override.yaml").getFile();
            System.setProperty("siren.config.path", path);
            Siren s = Siren.getInstance("test.siren.config.yaml");
            Assertions.assertEquals("com.stagwelltech.siren.util.SirenTestProviders$SirenProviderBasic", s.getProviderClassName());
        } finally {
            System.clearProperty("siren.config.path");
        }
    }
}
