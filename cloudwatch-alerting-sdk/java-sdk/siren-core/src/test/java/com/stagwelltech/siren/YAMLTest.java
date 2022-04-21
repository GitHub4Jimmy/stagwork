package com.stagwelltech.siren;
import com.stagwelltech.siren.util.YAML;
import com.stagwelltech.siren.util.YAMLTestModel;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;

import java.io.IOException;

public class YAMLTest {

    @Test
    @DisplayName("Loading YAML file to an application Model")
    public void testYamlModelNOBaseName() throws IOException {
        YAMLTestModel model = YAML.LoadClassPath("test.nobase.yaml", YAMLTestModel.class);
        Assertions.assertEquals(10, model.i);
        Assertions.assertEquals(1.456, model.f, 0.001f);
        Assertions.assertEquals(true, model.b);
        Assertions.assertEquals("hello, world", model.s);
    }

    @Test
    @DisplayName("Loading YAML file to an application Model")
    public void testYamlModelWITHBaseName() throws IOException {
        YAMLTestModel model = YAML.LoadClassPath("test.basename.yaml", "test", YAMLTestModel.class);
        Assertions.assertEquals(15, model.i);
        Assertions.assertEquals(12.5, model.f, 0.001f);
        Assertions.assertEquals(false, model.b);
        Assertions.assertEquals("world, hello", model.s);
    }

    @Test
    @DisplayName("Loading YAML file to an application Model")
    public void testAsFile() throws IOException {
        String path = getClass().getClassLoader().getResource("test.nobase.yaml").getFile();
        YAMLTestModel model = YAML.LoadFile(path, YAMLTestModel.class);
        Assertions.assertEquals(10, model.i);
        Assertions.assertEquals(1.456, model.f, 0.001f);
        Assertions.assertEquals(true, model.b);
        Assertions.assertEquals("hello, world", model.s);
    }

    @Test
    @DisplayName("Loading YAML file to an application Model")
    public void testAsFileBase() throws IOException {
        String path = getClass().getClassLoader().getResource("test.basename.yaml").getFile();
        YAMLTestModel model = YAML.LoadFile(path, "test", YAMLTestModel.class);
        Assertions.assertEquals(15, model.i);
        Assertions.assertEquals(12.5, model.f, 0.001f);
        Assertions.assertEquals(false, model.b);
        Assertions.assertEquals("world, hello", model.s);
    }

    @Test
    @DisplayName("Loading invalid file as yaml")
    public void testAsFileBadFileName() throws IOException {
        YAMLTestModel model = YAML.LoadFile("DoesNotExist", YAMLTestModel.class);
        Assertions.assertEquals(null, model);
    }

}
