package com.stagwelltech.siren.model;

import com.google.common.collect.Maps;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.Date;
import java.util.Map;

@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder(builderClassName = "Builder")
public class Metric {

    private String category;
    private String name;
    private Map<String, String> tags = Maps.newHashMap();
    private Double value;
    private Date ts;

}
