namespace VendorApi.Integration.Tests.Setup;

public abstract class IntegrationBase
{
    private readonly VendorApiFactory _webApplicationFactory;
    protected ExternalServicesMock ExternalServicesMock { get; }

    protected IntegrationBase()
    {
        ExternalServicesMock = new ExternalServicesMock();
        _webApplicationFactory = new VendorApiFactory(ExternalServicesMock);
    }

    protected HttpClient GetClient() => _webApplicationFactory.CreateClient();
}