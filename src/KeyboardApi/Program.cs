using System.Text;
using Ardalis.GuardClauses;
using KeyboardApi.Extensions;
using KeyboardApi.Repository.Auth;
using KeyboardApi.Repository.Vendor;
using KeyboardApi.Routes;
using KeyboardApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add Config
var config = ConfigExtensions.GetConfiguration();
builder.Services.AddConfigurationSupport(config);

// Auth Services
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

// Auth Middleware
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
        ValidIssuer = Guard.Against.NullOrEmpty(config["Jwt:Issuer"]),
        ValidAudience = Guard.Against.NullOrEmpty(config["Jwt:Audience"]),
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(Guard.Against.NullOrEmpty(config["Jwt:Key"]))),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

// Add fake db to container.
builder.Services.AddDbContext<VendorDbContext>(opt => opt.UseInMemoryDatabase("Vendor"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddTransient<IDataSeeder, DataSeeder>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
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

// remove default logging providers
builder.Logging.ClearProviders();

// Serilog configuration        
var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Setup Custom Routes
app.AddVendorRoutes();
app.AddAuthRoutes();

app.UseHttpsRedirection();

await SeedData(app);

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

async Task SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    Guard.Against.Null(scopedFactory);
    
    using (var scope = scopedFactory.CreateScope())
    {
        var dataSeeder = scope.ServiceProvider.GetService<IDataSeeder>();
        Guard.Against.Null(dataSeeder);
        
        await dataSeeder.Seed();
    }
    
    logger.Information("Test Data seeded");
}

// For integration Tests
namespace KeyboardApi
{
    public partial class Program { }
}
