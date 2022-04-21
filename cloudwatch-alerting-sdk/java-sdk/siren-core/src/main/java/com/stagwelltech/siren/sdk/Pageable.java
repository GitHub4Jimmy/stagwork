package com.stagwelltech.siren.sdk;

import java.util.List;

public interface Pageable<T> {
    List<T> getPage();
    int getPageNumber();
    long getPageCount();
}
