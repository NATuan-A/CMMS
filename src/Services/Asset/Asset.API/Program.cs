using Asset.API.Extensions;
using Asset.Application;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Asset.Infrastructure;
using Asset.Infrastructure.Persistence;
using Infrastructure.Middlewares;
using HealthChecks.UI.Client;
using Asset.API;

var builder = WebApplication.CreateBuilder(args);

try
{
    // Add services to the container.
    builder.Host.AddAppConfigurations();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices();
    builder.Services.ConfigureHealthChecks();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{ 
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
        $"{builder.Environment.ApplicationName} v1"));
    //}

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var assetContextSeed = scope.ServiceProvider.GetRequiredService<AssetContextSeed>();
        await assetContextSeed.InitialiseAsync();
        await assetContextSeed.SeedAsync();
    }

    app.UseMiddleware<ErrorWrappingMiddleware>();

    // app.UseHttpsRedirection(); //production only
    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        endpoints.MapDefaultControllerRoute();
    });

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}