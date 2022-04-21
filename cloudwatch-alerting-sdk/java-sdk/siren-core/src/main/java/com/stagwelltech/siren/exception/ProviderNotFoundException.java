package com.stagwelltech.siren.exception;

public class ProviderNotFoundException extends Exception {
    private String providerName;

    public ProviderNotFoundException(String providerName) {
        super("Missing provider class");
        this.providerName = providerName;
    }

    public ProviderNotFoundException(String providerName, Throwable e) {
        super("Missing provider class", e);
        this.providerName = providerName;
    }
}
