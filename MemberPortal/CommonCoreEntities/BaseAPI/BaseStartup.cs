using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Azure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StagwellTech.SEIU.CommonCoreEntities.Data;
using StagwellTech.ServiceBusRPC;
using StagwellTech.SEIU.CommonCoreEntities.AuthSEIU;
using Microsoft.OpenApi.Models;
using StagwellTech.SEIU.CommonCoreEntities.Services;
using StagwellTech.SEIU.CommonEntities.CorrectAddress;
using StackExchange.Redis;
//using System.Text.Json.Serialization;
//using System.Text.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;

namespace StagwellTech.SEIU.CommonCoreEntities.BaseAPI
{
    /*
    public class DoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && reader.GetString() == "NaN")
            {
                return double.NaN;
            }

            return reader.GetDouble(); // JsonException thrown if reader.TokenType != JsonTokenType.Number
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            if (double.IsNaN(value))
            {
                writer.WriteStringValue("NaN");
            }
            else
            {
                writer.WriteNumberValue(value);
            }
        }
    }

    public class DictionaryInt32Converter : JsonConverter<Dictionary<int, string>>
    {
        public override Dictionary<int, string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var value = new Dictionary<int, string>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return value;
                }

                string keyString = reader.GetString();

                if (!int.TryParse(keyString, out int keyAsInt32))
                {
                    throw new JsonException($"Unable to convert \"{keyString}\" to System.Int32.");
                }

                reader.Read();

                string itemValue = reader.GetString();

                value.Add(keyAsInt32, itemValue);
            }

            throw new JsonException("Error Occured");
        }
        public override void Write(Utf8JsonWriter writer, Dictionary<int, string> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (KeyValuePair<int, string> item in value)
            {
                writer.WriteString(item.Key.ToString(), item.Value);
            }

            writer.WriteEndObject();
        }
    }
    */
    public class BaseStartup
    {
        public const string MyAllowSpecificOrigins = "AllowAll";
        private static string _projectName { get; set; }
        public BaseStartup(IConfiguration configuration, string projectName)
        {
            Configuration = configuration;
            _projectName = projectName;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            var DatabaseConnectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
            var DNNDatabaseConnectionString = Environment.GetEnvironmentVariable("DNNDatabaseConnectionString");
            var ServiceBusConnectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
            var StorageConnectionString = Environment.GetEnvironmentVariable("StorageConnectionString");
            var RedisConnectionString = Environment.GetEnvironmentVariable("RedisConnectionString");

            if(string.IsNullOrEmpty(RedisConnectionString))
            {
                RedisConnectionString = "32bjdevdnn.redis.cache.windows.net:6380,password=Y4HVlEL69AM4r634yXTuzdzG7kxWgZkRT2Hcb1aThpk=,ssl=True,abortConnect=False";
            }

            if (String.IsNullOrEmpty(DatabaseConnectionString))
            {
                DatabaseConnectionString = Configuration.GetConnectionString("DefaultConnection");
            }
            if (String.IsNullOrEmpty(StorageConnectionString))
            {
                StorageConnectionString = Configuration.GetConnectionString("StorageConnectionString");
            }
            if (String.IsNullOrEmpty(DNNDatabaseConnectionString))
            {
                DNNDatabaseConnectionString = Configuration.GetConnectionString("DNNDatabaseConnectionString");
            }

            if (String.IsNullOrEmpty(DNNDatabaseConnectionString))
            {
                DNNDatabaseConnectionString = String.Copy(DatabaseConnectionString);
                DNNDatabaseConnectionString = DNNDatabaseConnectionString.Replace("32BJ_Member_Portal_Dev", "32BJ_DNN_Dev");
            }

            services.AddSingleton<IRedisClient, RedisClient>(opt =>
                RedisClient.Connect(RedisConnectionString)
            );
            //services.AddScoped<IRedisClient, RedisClient>(opt =>
            //    RedisClient.Connect(RedisConnectionString)
            //);

            services.AddDbContext<SeiuContext>(opt =>
                opt.UseSqlServer(DatabaseConnectionString, sqlServerOptionsAction: sqlOptions => {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromMilliseconds(500),
                        errorNumbersToAdd: null
                        );
                }),
                ServiceLifetime.Transient
                );

            services.AddDbContext<SeiuDNNContext>(opt =>
                opt.UseSqlServer(DNNDatabaseConnectionString, sqlServerOptionsAction: sqlOptions => {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromMilliseconds(500),
                        errorNumbersToAdd: null
                        );
                }),
                ServiceLifetime.Transient
                );

            services.AddSingleton<ServiceBusRPCService>(opt =>
                new ServiceBusRPCService(ServiceBusConnectionString));

            services.AddSingleton<ServiceBusRPCClient>(opt =>
                new ServiceBusRPCClient(ServiceBusConnectionString));
            
            services.AddSingleton<CloudStorageAccount>(opt =>
                CloudStorageAccount.Parse(StorageConnectionString));

            services.AddSingleton(opt =>
                new AddressStandartizationService());

            services.AddScoped<ISEIUAuthenticationService, SEIUAuthenticationService>();
            services.AddScoped<ProviderImageService>();
            services.AddScoped<FADTileIconService>();

            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddScoped<JHAsyncService>();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });
            });

            services.AddTransient<ICustomAuthenticationService, SEIUAuthenticationService>();

            services
                .AddSEIUAuthorization()
                .AddMvc(options =>
                {
                    var jsonInputFormatter = options.InputFormatters.OfType<SystemTextJsonInputFormatter>().First();
                    jsonInputFormatter.SupportedMediaTypes.Add("multipart/form-data");
                    options.EnableEndpointRouting = false;
                })
                .AddNewtonsoftJson(op => {
                    op.SerializerSettings.Converters.Add(new StringEnumConverter());
                });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SEIU API" + _projectName == null ? "" : (" - " + _projectName),
                });
            });

        }

    }
}
