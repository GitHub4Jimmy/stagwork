package com.stagwelltech.siren.sdk.impl;

import com.stagwelltech.siren.model.AlarmDefinition;
import com.stagwelltech.siren.sdk.Pageable;
import lombok.AccessLevel;
import lombok.Data;
import lombok.Getter;

import java.util.List;

@Data
public class CWPageable implements Pageable<AlarmDefinition> {

    @Getter(AccessLevel.NONE)
    private List<AlarmDefinition> definitions;

    private int pageNumber;

    public CWPageable(List<AlarmDefinition> definitions, int pageNumber) {
        this.definitions = definitions;
        this.pageNumber = pageNumber;
    }

    @Override
    public List<AlarmDefinition> getPage() {
        return definitions;
    }

    @Override
    public long getPageCount() {
        return definitions != null ? definitions.stream().count() : 0;
    }
}
