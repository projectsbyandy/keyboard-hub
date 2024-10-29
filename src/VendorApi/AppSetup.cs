using Ardalis.GuardClauses;
using VendorApi.Endpoints;
using VendorApi.Repository.Vendor;
using ILogger = Serilog.ILogger;

namespace VendorApi;

internal static class AppSetup
{
    public static WebApplication RegisterInternalEndpoints(this WebApplication app)
    {
        app.AddVendorEndpoints();
        app.AddAuthEndpoints();
        
        return app;
    }

    public static WebApplication ConfigureSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
    
    public static async Task SeedDataAsync(this WebApplication app)
    {
        var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
        Guard.Against.Null(scopedFactory);
    
        using (var scope = scopedFactory.CreateScope())
        {
            var dataSeeder = scope.ServiceProvider.GetService<IDataSeeder>();
            Guard.Against.Null(dataSeeder);
        
            await dataSeeder.Seed();
        }
    
        app.Services.GetService<ILogger>()?.Information("Test Data seeded");
    }
}