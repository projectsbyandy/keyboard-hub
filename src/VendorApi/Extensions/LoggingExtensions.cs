using Serilog;

namespace VendorApi.Extensions;

internal static class LoggingExtensions
{
    public static WebApplicationBuilder AddLoggingSupport(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Services.AddSerilog(sp =>
            sp
                .MinimumLevel.Debug()
                .WriteTo.Console());
        return builder;
    }
}