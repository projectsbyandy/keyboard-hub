using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VendorApi.Integration.Tests.Setup;

public class VendorApiFactory(ExternalServicesMock externalServicesMock) : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var dummyHost = builder.Build();
        
        builder.UseEnvironment("IntegrationTesting");
        builder
            .ConfigureServices(services =>
            {
                services.AddLogging(loggingBuilder => loggingBuilder.AddConsole().AddDebug());
                foreach (var (interfaceType, serviceMock) in externalServicesMock.GetMocks())
                {
                    var service = services.SingleOrDefault(d => d.ServiceType == interfaceType);
                    
                    services.Remove(Guard.Against.Null(service));

                    services.AddSingleton(interfaceType, serviceMock);
                }
            });
        
        var host = builder.Build();
        host.Start();

        dummyHost.Start();
        
        return host;
    }
}