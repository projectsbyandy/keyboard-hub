using System.Text;
using KeyboardApi;
using KeyboardApi.Repository.Auth;
using KeyboardApi.Repository.Vendor;
using KeyboardApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Auth Services
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
// Auth Middleware
var test = builder.Configuration["Jwt:Issuer"];
var test1 = builder.Configuration["Jwt:Audience"];
var test3 = builder.Configuration["Jwt:Key"];
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

// Add fake db to container.
builder.Services.AddDbContext<VendorDbContext>(opt => opt.UseInMemoryDatabase("Vendor"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddTransient<IDataSeeder, DataSeeder>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// remove default logging providers
builder.Logging.ClearProviders();

// Serilog configuration        
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Setup Custom Routes
app.AddVendorRoutes();
app.AddAuthRoutes(builder);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await SeedData(app);

app.UseAuthentication();
app.UseAuthorization();

app.Run();

async Task SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory?.CreateScope())
    {
        var dataSeeder = scope?.ServiceProvider.GetService<IDataSeeder>();
        await dataSeeder.Seed();
    }
    
    logger.Information("Test Data seeded");
}
