package com.stagwelltech.siren.model;

import com.google.common.collect.Maps;
import lombok.Builder;
import lombok.Data;
import lombok.NonNull;

import java.util.Map;

@Data
@Builder(builderClassName = "Builder")
public class AlarmDefinition {
    private String category;
    private String name;
    private String description;
    private String metricName;
    private AlarmSpec spec;
    private Map<String, String> tags = Maps.newHashMap();
}
