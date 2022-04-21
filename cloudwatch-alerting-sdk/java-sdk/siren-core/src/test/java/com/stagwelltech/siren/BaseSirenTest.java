package com.stagwelltech.siren;

import org.junit.jupiter.api.BeforeEach;

import java.lang.reflect.Field;
import java.util.Arrays;

public abstract class BaseSirenTest {

    @BeforeEach
    protected void clearSirenInstance() throws IllegalAccessException {
        Field f = Arrays.stream(Siren.class.getDeclaredFields()).filter(field -> {
            return field.getName().equals("instance");
        }).findFirst().get();

        f.setAccessible(true);
        f.set(null, null);
    }

}
