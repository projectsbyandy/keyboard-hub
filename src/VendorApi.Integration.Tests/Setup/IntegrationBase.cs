namespace VendorApi.Integration.Tests.Setup;

public abstract class IntegrationBase
{
    private readonly KeyboardApiFactory _webApplicationFactory;
    protected ExternalServicesMock ExternalServicesMock { get; }

    protected IntegrationBase()
    {
        ExternalServicesMock = new ExternalServicesMock();
        _webApplicationFactory = new KeyboardApiFactory(ExternalServicesMock);
    }

    protected HttpClient GetClient() => _webApplicationFactory.CreateClient();
}