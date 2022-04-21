package com.stagwelltech.siren.spring.util;

import com.google.common.collect.Sets;
import org.springframework.aop.framework.AopProxyUtils;
import org.springframework.context.ApplicationContext;

import java.lang.annotation.Annotation;
import java.lang.reflect.Method;
import java.util.Set;

//https://stackoverflow.com/questions/27929965/find-method-level-custom-annotation-in-a-spring-context
public class AnnotationScanner {

    public static class AnnotatedMethod {
        public Class<?> type;
        public Object instance;
        public Method method;

        public AnnotatedMethod(Class<?> type, Object instance, Method method) {
            this.type = type;
            this.instance = instance;
            this.method = method;
        }
    }

    public static Set<AnnotatedMethod> FindAnnotatedMethods(ApplicationContext applicationContext, Class<? extends Annotation> annotation) throws Exception {

        Set<AnnotatedMethod> results  = Sets.newHashSet();

        for (String beanName : applicationContext.getBeanDefinitionNames()) {

            //we need to handle prototype beans differently because calling getBean() will create a  new instance of them which is not desirable
            //specifically if that bean is wired through an InjectionPoint
            if (applicationContext.isPrototype(beanName)) {
                //it doesnt make sense to use this on a prototype bean as it would only apply to instances that exist at call-time
                continue;
            }

            Object obj = applicationContext.getBean(beanName);
            results.addAll(getAnnotationsFromBean(applicationContext, obj, annotation));
        }

        return results;
    }

    public static Set<AnnotatedMethod> getAnnotationsFromBean(ApplicationContext applicationContext, Object obj, Class<? extends Annotation> annotation) throws ClassNotFoundException {

        Set<AnnotatedMethod> results  = Sets.newHashSet();

        Object proxied = AopProxyUtils.getSingletonTarget(obj);
        if (proxied != null) {
            obj = proxied;
        }

        /*
         * As you are using AOP check for AOP proxying. If you are proxying with Spring CGLIB (not via Spring AOP)
         * Use org.springframework.cglib.proxy.Proxy#isProxyClass to detect proxy If you are proxying using JDK
         * Proxy use java.lang.reflect.Proxy#isProxyClass
         */
        Class<?> objClz = AopProxyUtils.ultimateTargetClass(obj);

        //TODO: this is a gross way to find a proxy object
        if (objClz.getName().contains("EnhancerBySpringCGLIB")) {
            String actualClassName = objClz.getName().replaceAll("\\$\\$EnhancerBySpringCGLIB\\$\\$.*", "");
            Class<?> newType = Class.forName(actualClassName);
            objClz = newType;
            obj = objClz.cast(obj);
        }

        Method[] methods = objClz.getDeclaredMethods();

        for (Method m : methods) {
            if (m.isAnnotationPresent(annotation)) {
                results.add(new AnnotatedMethod(objClz, obj, m));
            }
        }

        return results;
    }
}