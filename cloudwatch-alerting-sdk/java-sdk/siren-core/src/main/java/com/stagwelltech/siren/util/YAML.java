package com.stagwelltech.siren.util;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.dataformat.yaml.YAMLFactory;

import java.io.File;
import java.io.IOException;
import java.io.InputStream;

public class YAML {

    private static final ObjectMapper mapper = new ObjectMapper(new YAMLFactory());

    static {
        mapper.findAndRegisterModules();
        mapper.setSerializationInclusion(JsonInclude.Include.NON_NULL);
    }

    private static File getFile(String filePath) {
        File f = new File(filePath);
        if (!f.exists() || !f.canRead()) {
            return null;
        }
        return f;
    }

    private static InputStream getClassPath(String filePath) {
        return YAML.class.getClassLoader().getResourceAsStream(filePath);
    }

    @SuppressWarnings("unchecked")
    public static <T> T LoadFile(String filePath, Class<T> typeClass) throws IOException {
        File f = getFile(filePath);
        if (f == null) {
            return null;
        }
        return (T)mapper.readerFor(typeClass).readValue(f);
    }

    @SuppressWarnings("unchecked")
    public static <T> T LoadFile(String filePath, String root, Class<T> typeClass) throws IOException {
        File f = getFile(filePath);
        if (f == null) {
            return null;
        }
        return (T)mapper.readerFor(typeClass).withRootName(root).readValue(f);
    }

    @SuppressWarnings("unchecked")
    public static <T> T LoadClassPath(String filePath, Class<T> typeClass) throws IOException {
        try (InputStream stream = getClassPath(filePath)) {
            if (stream == null) {
                return null;
            }
            return (T) mapper.readerFor(typeClass).readValue(stream);
        }
    }

    @SuppressWarnings("unchecked")
    public static <T> T LoadClassPath(String filePath, String root, Class<T> typeClass) throws IOException {
        try (InputStream stream = getClassPath(filePath)) {
            if (stream == null) {
                return null;
            }
            return (T) mapper.readerFor(typeClass).withRootName(root).readValue(stream);
        }
    }

    public static void WriteFile(String filePath, String root, Object data) throws IOException {
        mapper.writerFor(data.getClass()).withRootName(root).writeValue(new File(filePath), data);
    }
}
