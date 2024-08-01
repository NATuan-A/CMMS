using Contracts.Domains.Interfaces;
using Contracts.Services;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Asset.Application.Common.Interfaces;
using Shared.Configurations;
using Asset.Infrastructure.Persistence;
using Asset.Infrastructure.Repositories;
using Asset.Application.Services.Interfaces;
using Asset.Application.Services;

namespace Asset.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            if (databaseSettings == null || string.IsNullOrEmpty(databaseSettings.ConnectionString))
                throw new ArgumentNullException("Connection string is not configured.");

            services.AddDbContext<AssetContext>(options =>
            {
                options.UseSqlServer(databaseSettings.ConnectionString,
                    builder =>
                        builder.MigrationsAssembly("Asset.API"));
            });

            services.AddTransient<AssetContextSeed>();
            services.AddScoped<IAssetService, AssetService>()
                .AddScoped<IAssetRepository, AssetCMMSRepository>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            //services.AddScoped(typeof(ISmtpEmailService), typeof(SmtpEmailService));

            return services;
        }
    }
}
