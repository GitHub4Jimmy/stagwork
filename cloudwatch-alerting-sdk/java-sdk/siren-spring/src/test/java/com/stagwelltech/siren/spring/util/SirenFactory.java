package com.stagwelltech.siren.spring.util;

import com.stagwelltech.siren.BaseAWSTest;
import com.stagwelltech.siren.Siren;
import com.stagwelltech.siren.exception.ConfigMissingException;
import com.stagwelltech.siren.exception.InitializationException;
import com.stagwelltech.siren.exception.ObjectMergeFailed;
import com.stagwelltech.siren.exception.ProviderNotFoundException;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.io.IOException;

@Configuration
public class SirenFactory {

    @Bean
    public Siren createSiren() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException {
        return Siren.getInstance(BaseAWSTest.tempConfigFileName);
    }
}
