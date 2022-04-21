package com.stagwelltech.siren;

import com.stagwelltech.siren.config.SirenConfig;
import com.stagwelltech.siren.exception.ConfigMissingException;
import com.stagwelltech.siren.exception.InitializationException;
import com.stagwelltech.siren.exception.ObjectMergeFailed;
import com.stagwelltech.siren.exception.ProviderNotFoundException;
import com.stagwelltech.siren.sdk.SirenSDK;
import com.stagwelltech.siren.util.BeanUtils;
import com.stagwelltech.siren.util.YAML;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;
import java.lang.reflect.InvocationTargetException;
import java.util.function.Function;

public class Siren {

    private static Siren instance = null;

    private static final Logger log = LoggerFactory.getLogger(Siren.class);

    private static final String DEFAULT_CONFIG_FILE = "siren.config.yaml";
    private String overrideConfigFile = null;

    private static final String DEFAULT_EXTERNAL_CONFIG_LOCATION = "/tmp/" + DEFAULT_CONFIG_FILE;
    private static final String DEFAULT_PATH_PROPERTY = "siren.config.path";

    private SirenConfig config;

    private SirenSDK sdk;

    private Siren() throws IOException, ConfigMissingException, ProviderNotFoundException, ObjectMergeFailed, InitializationException {
        overrideConfigFile = null;
        loadConfig();
        initializeProvider();
    }

    private Siren(String overrideConfigFile) throws IOException, ConfigMissingException, ProviderNotFoundException, ObjectMergeFailed, InitializationException {
        this.overrideConfigFile = overrideConfigFile;
        loadConfig();
        initializeProvider();
    }

    public static synchronized Siren getInstance() throws IOException, ConfigMissingException, ProviderNotFoundException, ObjectMergeFailed, InitializationException {
        if (instance == null) {
            instance = new Siren();
        }
        return instance;
    }

    public static synchronized Siren getInstance(String overrideConfigFile) throws IOException, ConfigMissingException, ProviderNotFoundException, ObjectMergeFailed, InitializationException {
        if (instance == null) {
            instance = new Siren(overrideConfigFile);
        }
        return instance;
    }

    private void loadConfig() throws IOException, ConfigMissingException, ObjectMergeFailed {

        config = YAML.LoadClassPath(DEFAULT_CONFIG_FILE, "siren", SirenConfig.class);
        if (config == null) {
            throw new ConfigMissingException();
        }

        if (overrideConfigFile != null) {
            SirenConfig overrideFile = YAML.LoadClassPath(overrideConfigFile, "siren", SirenConfig.class);
            if (overrideFile == null) {
                overrideFile = YAML.LoadFile(overrideConfigFile, "siren", SirenConfig.class);
                if (overrideFile == null) {
                    throw new ConfigMissingException();
                }
            }
            BeanUtils.merge(config, overrideFile);
        }

        String pathConfigured = System.getProperty(DEFAULT_PATH_PROPERTY);
        if (pathConfigured == null) {
            pathConfigured = DEFAULT_EXTERNAL_CONFIG_LOCATION;
        }

        SirenConfig overrideConfig = YAML.LoadFile(pathConfigured, "siren", SirenConfig.class);
        if (overrideConfig != null) {
            BeanUtils.merge(config, overrideConfig);
        }

        log.info("Loading monitor provider [{}]", config.provider);
    }

    public String getProviderClassName() {
        return config.provider;
    }

    public SirenSDK getSDK() {
        return sdk;
    }

    public boolean isEnabled() { return config.enabled; }

    @SuppressWarnings("unchecked")
    private void initializeProvider() throws ProviderNotFoundException, InitializationException {
        Class<? extends SirenSDK> clazz = null;
        try {
            clazz = (Class<? extends SirenSDK>) Class.forName(config.provider);
        } catch (ClassNotFoundException e) {
            log.error("Class not found for [{}]", config.provider);
        }
        if (clazz == null) {
            throw new ProviderNotFoundException(config.provider);
        }

        try {
            sdk = clazz.getDeclaredConstructor().newInstance();
            sdk.setConfig(config);
            sdk.initialize();
        } catch (NoSuchMethodException nsme) {
            log.error("No constructor found for provider [{}]", config.provider);
            throw new ProviderNotFoundException(config.provider, nsme);
        } catch (IllegalAccessException e) {
            log.error("Illegal Access for constructor on [{}]", config.provider);
            throw new ProviderNotFoundException(config.provider, e);
        } catch (InstantiationException e) {
            log.error("Instantiation Exception for [{}]", config.provider);
            throw new ProviderNotFoundException(config.provider, e);
        } catch (InvocationTargetException e) {
            log.error("Invocation Target Exception for [{}]", config.provider);
            throw new ProviderNotFoundException(config.provider, e);
        }
    }

}
