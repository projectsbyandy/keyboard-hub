using System.Text;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VendorApi.Extensions;
using VendorApi.Repository.Auth;
using VendorApi.Repository.Auth.Internal;
using VendorApi.Repository.Vendor;
using VendorApi.Repository.Vendor.Internal;
using VendorApi.Services;
using VendorApi.Services.Internal;

namespace VendorApi;

public static class BuilderSetup
{
    private static IConfiguration? _config;
    
    public static WebApplicationBuilder ConfigureAuthMiddleware(this WebApplicationBuilder builder)
    {
        Guard.Against.Null(_config, "config empty, ensure that the config support has been added");
        
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = Guard.Against.NullOrEmpty(_config["Jwt:Issuer"]),
                ValidAudience = Guard.Against.NullOrEmpty(_config["Jwt:Audience"]),
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(Guard.Against.NullOrEmpty(_config["Jwt:Key"]))),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureFakeDb(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<VendorDbContext>(opt => opt.UseInMemoryDatabase("Vendor"));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddTransient<IDataSeeder, DataSeeder>();

        return builder;
    }

    public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Vendor Services", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return builder;
    }

    public static WebApplicationBuilder AddInternalServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IUserRepository, UserRepository>()
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IVendorService, VendorService>();

        return builder;
    }

    public static WebApplicationBuilder AddConfigSupport(this WebApplicationBuilder builder)
    {
        _config = ConfigExtensions.GetConfiguration();
        builder.Services.AddConfigurationSupport(_config);

        return builder;
    }

    public static WebApplicationBuilder AddCorConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy("VendorFrontEnd", policyBuilder =>
            {
                var frontEndUrl = Guard.Against.NullOrEmpty(builder.Configuration["FrontendUrl"]);
                Console.WriteLine("This is the front end url: " + frontEndUrl);
                policyBuilder.WithOrigins(frontEndUrl).AllowAnyHeader().AllowAnyMethod();
            });
        });
        
        return builder;
    }
}