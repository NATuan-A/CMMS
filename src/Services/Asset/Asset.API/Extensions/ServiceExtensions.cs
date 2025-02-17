﻿using AutoMapper;
using AutoMapper.Internal;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared.Configurations;

namespace Asset.API.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            //var emailSettings = configuration.GetSection(nameof(SMTPEmailSetting))
            //    .Get<SMTPEmailSetting>();
            //services.AddSingleton(emailSettings);

            var databaseSettings = configuration.GetSection(nameof(DatabaseSettings))
                .Get<DatabaseSettings>();
            services.AddSingleton(databaseSettings);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.Internal().MethodMappingEnabled = false;
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

        public static void ConfigureHealthChecks(this IServiceCollection services)
        {
            var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            services.AddHealthChecks()
                .AddSqlServer(databaseSettings.ConnectionString,
                    name: "SqlServer Health",
                    failureStatus: HealthStatus.Degraded);
        }
    }
}
