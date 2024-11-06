namespace VendorApi.Integration.Tests.Setup;

public abstract class IntegrationBase
{
    protected readonly VendorApiFactory WebApplicationFactory;
    protected ExternalServicesMock ExternalServicesMock { get; }

    protected IntegrationBase()
    {
        ExternalServicesMock = new ExternalServicesMock();
        WebApplicationFactory = new VendorApiFactory(ExternalServicesMock);
    }

    protected HttpClient GetClient() => WebApplicationFactory.CreateClient();
}