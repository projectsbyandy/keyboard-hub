using VendorApi;
using VendorApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggingSupport();

builder
    .AddConfigSupport()
    .AddInternalServices()
    .ConfigureAuthMiddleware()
    .ConfigureFakeDb()
    .ConfigureSwagger();

var app = builder.Build();

app
    .RegisterInternalEndpoints()
    .ConfigureSwagger()
    .UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization();

await app.SeedDataAsync();

app.Run();

namespace VendorApi
{
    public partial class Program { }
}
