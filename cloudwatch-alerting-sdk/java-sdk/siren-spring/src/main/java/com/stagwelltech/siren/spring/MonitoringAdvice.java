package com.stagwelltech.siren.spring;

import com.stagwelltech.siren.Siren;
import com.stagwelltech.siren.exception.*;
import com.stagwelltech.siren.model.Metric;
import com.stagwelltech.siren.sdk.annotation.Monitored;
import com.stagwelltech.siren.sdk.metric.ValueResolver;
import org.aspectj.lang.ProceedingJoinPoint;
import org.aspectj.lang.annotation.Around;
import org.aspectj.lang.annotation.Aspect;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.lang.reflect.InvocationTargetException;
import java.util.Date;

@Aspect
@Component
public class MonitoringAdvice {

    private static final Logger log = LoggerFactory.getLogger(MonitoringAdvice.class);

    @Autowired
    private final Siren siren = null;

    /**
     * This is used for unit testing
     */
    private Throwable lastExceptionRaised = null;

    public synchronized Throwable getLastException() {
        Throwable t = lastExceptionRaised;
        lastExceptionRaised = null;
        return t;
    }

    @Around("@annotation(def)")
    public Object logAction(ProceedingJoinPoint pjp, Monitored def) throws Throwable {

        if (!siren.isEnabled()) {
            return pjp.proceed();
        }

        Object result = null;
        Metric.Builder m = Metric.builder();

        m.category(def.category())
         .name(def.metricName());

        log.debug("Logging metric for method [{}]", pjp.toLongString());

        try {
            result = pjp.proceed();
            m.ts(new Date())
             .value(getResultValue(result, def));

            siren.getSDK().log(m.build());

        } catch (MetricException me) {
            log.error("Metric Exception", me);
            lastExceptionRaised = me;
        } catch (Throwable t) {
            log.error("Error executing metric logger method " + pjp.toLongString(), t);
            lastExceptionRaised = t;
            throw t;
        }

        return result;
    }

    public Double getResultValue(Object result, Monitored def) throws MetricException {
        try {
            ValueResolver resolver = def.valueResolver().getConstructor().newInstance();
            return resolver.resolveValue(result, def);
        } catch (InstantiationException | IllegalAccessException | InvocationTargetException | NoSuchMethodException e) {
            log.error("Cant create ValueResolver", e);
            throw new MetricException(e);
        }
    }
}
