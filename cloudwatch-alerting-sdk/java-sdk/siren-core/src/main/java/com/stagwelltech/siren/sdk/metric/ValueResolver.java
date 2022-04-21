package com.stagwelltech.siren.sdk.metric;

import com.stagwelltech.siren.exception.MetricException;
import com.stagwelltech.siren.sdk.annotation.Monitored;

public interface ValueResolver {
    Double resolveValue(Object input, Monitored monitor) throws MetricException;
}
