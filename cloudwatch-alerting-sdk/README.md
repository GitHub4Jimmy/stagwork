# cloudwatch-alerting-sdk
Automatically fire custom metrics to AWS CloudWatch. Automatic alert configuration and creation.

## Installation
You will need to set up your maven environment to work with github packages. See [Setting Up Your Envioronment for GitHub Packages](packages.md).

    <dependency>
      <groupId>com.stagwelltech</groupId>
      <artifactId>siren-core</artifactId>
      <version>1.0.0</version>
    </dependency>

## Spring integration
Spring integration is shipped as a separate module. As it only uses a compile time dependency on siren-core, you must have both dependencies in your pom.xml.

    <dependency>
      <groupId>com.stagwelltech</groupId>
      <artifactId>siren-spring</artifactId>
      <version>1.0.0</version>
    </dependency> 
    
## Configuration
Siren requires access to AWS cloudwatch. It will use either environment variables, a profile specifier, or access keys depending on how it is configured.

### Default config file
By default, the configuration file can be overriden at `/tmp/siren.config.yaml`. You can change the location by passing `-Dsiren.config.path=/path/to/config/file` to the java process.

The properties supported in the config file are as follows (the access key and secret key are fake):
    
    ---
    siren:
      enabled: true
      provider: com.stagwelltech.siren.sdk.impl.SirenSDKCWImpl
      async: true
      asyncWaitTime: 500
      aws:
        region: us-east-1
        httpConnectionsCount: 150
        endpoint: https://monitoring.us-east-1.amazonaws.com
        profile: default
        accessKey: A000000000000000001
        secretKey: KLHJ9845234UFHN3F908342F90
        
Typically you would use environment variables or a profile to load the aws credentials. Directly using access keys and secret keys is not recommended. *Do not* commit your config file to a repository.

## Usage
Usage of the library requires minimal setup.

    //create an instance of siren to use. Siren is a singleton, you should manage its lifecycle appropriately.
    Siren s = Siren.getInstance();
    
    Metric.Builder m = Metric.builder();
            m.category("Metric Namespace")
                    .name("Metric Name")
                    .ts(new Date())
                    .value(101.1)           // the actuial metric value
                    .tags(                  //an arbitrary list of tags, or dimensions for this metric
                        Stream.of(
                            new String[][] {
                                { "Tag1", "1" },
                                { "Tag2", "2" }
                            }
                        )
                        .collect(
                            Collectors.toMap(data -> data[0], data -> data[1])
                        )
                    );
                    
    //log the custom metric
    s.getSDK().log(m.build());
    

#### Spring
With spring, you need to create a factory for the Siren object so it is loaded into the context.

    @Configuration
    public class SirenFactory {
    
        @Bean
        public Siren createSiren() throws InitializationException, ObjectMergeFailed, ConfigMissingException, ProviderNotFoundException, IOException {
            return Siren.getInstance();
        }
    }
    
You may need to add the package `com.stagwelltech.siren.spring` to your application component scanning paths.

Add `@EnableAspectJAutoProxy` to your Spring Boot main class, or your application context xml.

Now you annotate service methods you would like metrics on with `@Monitored`. Only methods with

    @Service
    public class MonitorMethods {
    
        private static final Logger log = LoggerFactory.getLogger(MonitorMethods.class);
    
        @Monitored(category = "Metric Namespace", metricName = "Metric Name")
        public Integer methodWithTracking() {
            log.info("Invocations of this method are logged as a custom metric.");
            return 11;
        }
    }
    
Creating ana alarm uses the exact same setup, with some additional information on the annotation.

    @Monitored(category = "Metric Namespace", metricName = "Metric Name", alarmName = "Alarm Name", description = "Alarm Description", alarmThreshold = 20)
    public Integer fireLogIntegerAlert() {
        return 15;
    }
    
The full list of options on alarms is as follows:

    @Data
    @Builder(builderClassName = "Builder")
    public class AlarmSpec {
    
        /**
         * How many data points need to be outside our defined threshold in order to fire an alarm
         */
        private Integer numberOfPointsToAlarm;
    
        /**
         * How many points are we checking for alarm conditions. This is the number of points in the bucket, against which we check for numberOfPointsToAlarm points which breach our threshold.
         */
        private Integer numberOfPointsToCheck;
    
        /**
         * The time in seconds between data points.
         */
        private Integer timeBetweenDataPoints;
    
        /**
         * How to process the data between data points to create a single data point.
         */
        private AlarmAggregateType aggregation;
    
        /**
         * Provider specific string describing the units of measurement.
         * AWS CloudWatch Provider - Evaluates to StandardUnit.fromValue(alarmUnit) before sending to CW
         */
        private String alarmUnit;
    
        /**
         * How to test against the alarm threshold
         */
        private Comparison thresholdComparison;
    
        /**
         * The actual threshold value
         */
        private Double alarmThreshold;
    
        /**
         * How to handle missing data
         */
        private MissingData missingData;
    }

# Running LocalStack Unit Tests
Local stack unit tests are diabled by default. To enable them, follow these steps:

1. Install the docker CLI on your local machine and add it to your PATH
2. Make sure your local docker is running properly, or you have set the appropriate IP/Hostname using DOCKER_HOST
3. Create an environment variable [DOCKER_LOCATION] pointing at your docker cli executable (C:\docker\docker.exe for example)
4. Run the unit tests - you may need to restart your terminal or IDE after setting the env variables.
5. Profit
