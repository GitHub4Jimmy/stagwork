package com.stagwelltech.siren.sdk.metric.impl;

import com.google.common.collect.Sets;
import com.stagwelltech.siren.exception.MetricException;
import com.stagwelltech.siren.sdk.annotation.Monitored;
import com.stagwelltech.siren.sdk.metric.ValueResolver;

import java.util.Set;

public class ScalarTypeValueResolver implements ValueResolver {

    private static final Set<Class<?>> scalarTypes = Sets.newHashSet(
            Byte.class,
            Short.class,
            Integer.class,
            Long.class,
            Float.class,
            Double.class,
            Boolean.class
    );

    @Override
    public Double resolveValue(Object input, Monitored monitor) throws MetricException {
        Class<?> inputType = input.getClass();
        if (inputType.isPrimitive() || scalarTypes.contains(inputType)) {
            if (inputType.equals(Character.TYPE)) {
                throw new MetricException("Cannot convert value type from [char]");
            }
            if (inputType.equals(Boolean.TYPE) || inputType.equals(Boolean.class)) {
                return ((Boolean)input) ? 1.0 : 0;
            }
            return ((Number)input).doubleValue();
        }
        throw new MetricException("Cannot convert value type from [" + inputType.getName() + "]");
    }
}
