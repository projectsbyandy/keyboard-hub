using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Moq;
using VendorApi.Integration.Tests.Setup;
using VendorApi.Models.Auth;

namespace VendorApi.Integration.Tests;

[Parallelizable(ParallelScope.Fixtures)]
public class AuthApiTests : IntegrationBase
{
     private HttpClient _client;
     private static readonly SemaphoreSlim Semaphore = new(1);
     private const int SemaphoreMaxWait = 1000;

     [SetUp]
     public void SetUp()
     {
         _client = GetClient();
     }

    #region Basic Api Auth Tests
    
    [TestCase("Mary", "doesnotexist@test.com", "tester456", HttpStatusCode.Unauthorized)]
    [TestCase("Mary", "mary.rogers@test.com", "tester456", HttpStatusCode.OK)]
    [TestCase("Andy", "andy.peters@test.com", "tester123", HttpStatusCode.OK)]
    public async Task Verify_Authentication_Test(string username, string email, string password, HttpStatusCode expectedStatusCode)
    {
        // Arrange / Act
        var response = await CallAuthAsync(username, email, password);
        
        // Assert
        response.StatusCode.Should().Be(expectedStatusCode);

        var test = ExternalServicesMock.GetMocks();
    }
    
    [TestCase("Mary", "mary.rogers@test.com", "tester456")]
    [TestCase("Andy", "andy.peters@test.com", "tester123")]
    public async Task Verify_Generated_Token_Length_Test(string username, string email, string password)
    {
        // Arrange / Act
        var response = await CallAuthAsync(username, email, password);
        var token = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        token.Length.Should().Be(333);
    }
    
    [TestCase("Mary", "doesnotexist@testing.com", "tester456")]
    [TestCase("Mary", "alsonothere.rogers@test.com", "tester456")]
    [TestCase("Andy", "nope@test.com", "tester123")]
    public async Task Verify_InvalidUser_Returns_401_With_Logging(string username, string email, string password)
    {
        // Arrange / Act
        var response = await CallAuthAsync(username, email, password);
        
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        responseContent.Should().BeEmpty();
        ExternalServicesMock.Logger.Verify(x => x.Warning("Unable to login using {Email}, due to {LoginOutcome}", email, LoginOutcome.NotFound), Times.Once);
    }
    
    [TestCase("Mary", "mary.rogers@test.com", "guess")]
    [TestCase("Andy", "andy.peters@test.com", "randompass")]
    public async Task Verify_Invalid_Password_Returns_401_And_Logs(string username, string email, string password)
    {
        // Arrange / Act
        var response = await CallAuthAsync(username, email, password);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        ExternalServicesMock.Logger.Verify(x => x.Warning("Unable to login using {Email}, due to {LoginOutcome}", email, LoginOutcome.InvalidPassword), Times.Once);
    }

    #endregion

    #region Endpoints requiring auth tests

    [TestCase("Mary", "mary.rogers@test.com", "tester456")]
    [TestCase("Andy", "andy.peters@test.com", "tester123")]
    public async Task Verify_Get_Vendor_With_Valid_Auth(string username, string email, string password)
    {
        // Given
        var response = await CallAuthAsync("Andy", "andy.peters@test.com", "tester123");
        var token = await response.Content.ReadAsStringAsync();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        // When
        var vendors = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/vendor/all"));
        
        // Then
        vendors.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Test]
    public async Task Verify_Get_Vendor_With_Auth_Without_Token()
    {
        // Given / When
        var vendors = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/vendor/all"));
        
        // Then
        vendors.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    #endregion

    private async Task<HttpResponseMessage> CallAuthAsync(string username, string email, string password)
    {
        await Semaphore.WaitAsync(SemaphoreMaxWait);

        try
        {
            return await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/authenticate/login")
            {
                Content = new StringContent(JsonSerializer.Serialize(new User
                {
                    Username = username,
                    Email = email,
                    Password = password
                }), Encoding.UTF8, "application/json")
            });
        }
        finally
        {
            Semaphore.Release();
        }
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }
}