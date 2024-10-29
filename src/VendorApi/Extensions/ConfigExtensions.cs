using Ardalis.GuardClauses;
using VendorApi.Models.Config;

namespace VendorApi.Extensions;

public static class ConfigExtensions
{
    public static IServiceCollection AddConfigurationSupport(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var seedUsers = configuration.GetSection("Users").Get<SeedUsers>();
        var jwtConfig = configuration.GetSection("Jwt").Get<JwtConfig>();

        Guard.Against.Null(seedUsers);
        Guard.Against.Null(jwtConfig);
        
        serviceCollection.AddTransient(_ => seedUsers);
        serviceCollection.AddTransient(_ => jwtConfig);
        
        return serviceCollection;
    }
    
    public static IConfiguration GetConfiguration()
    {
        var env = Environment.GetEnvironmentVariable("ENVIRONMENTINTEST") ?? "development";

        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false)
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
    }
}