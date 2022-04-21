package com.stagwelltech.siren.exception;

public class MetricException extends Exception {
    public MetricException() {
    }

    public MetricException(String message) {
        super(message);
    }

    public MetricException(String message, Throwable cause) {
        super(message, cause);
    }

    public MetricException(Throwable cause) {
        super(cause);
    }

    public MetricException(String message, Throwable cause, boolean enableSuppression, boolean writableStackTrace) {
        super(message, cause, enableSuppression, writableStackTrace);
    }
}
