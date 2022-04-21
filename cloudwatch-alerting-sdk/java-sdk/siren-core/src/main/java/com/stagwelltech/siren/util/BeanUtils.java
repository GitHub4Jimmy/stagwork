package com.stagwelltech.siren.util;

import com.stagwelltech.siren.exception.ObjectMergeException;
import com.stagwelltech.siren.exception.ObjectMergeFailed;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.util.Arrays;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;

public class BeanUtils {

    public static void merge(Object dest, Object src) throws ObjectMergeFailed {
        List<Field> dstFields = Arrays.stream(dest.getClass().getDeclaredFields()).collect(Collectors.toList());

        try {
            dstFields.forEach(f -> {
                try {
                    int modifiers = f.getModifiers();
                    if (Modifier.isFinal(modifiers) | Modifier.isStatic(modifiers)) {
                        return;
                    }

                    f.setAccessible(true);

                    Field copy = src.getClass().getDeclaredField(f.getName());
                    copy.setAccessible(true);

                    Object value = copy.get(src);
                    if (value != null) {
                        //TODO: implement collections
                        if (!value.getClass().isAssignableFrom(Collection.class)) {
                            f.set(dest, value);
                        }
                    }

                } catch (NoSuchFieldException | IllegalAccessException e) {
                    throw new ObjectMergeException("Could not merge objects", e);
                }
            });
        } catch (ObjectMergeException e) {
            //convert this to a checked exception
            throw new ObjectMergeFailed(e.getMessage(),e);
        }
    }

}
