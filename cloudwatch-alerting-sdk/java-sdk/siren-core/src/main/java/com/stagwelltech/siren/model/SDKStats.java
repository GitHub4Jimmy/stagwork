package com.stagwelltech.siren.model;

import lombok.AllArgsConstructor;
import lombok.Data;

@Data
@AllArgsConstructor
public class SDKStats {
    private Long processedCount;
    private Long errorCount;
}
