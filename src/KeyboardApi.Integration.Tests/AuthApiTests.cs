using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using KeyboardApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace KeyboardApi.Integration.Tests;

public class AuthApiTests
{
     private HttpClient sut;
     
    [SetUp]
    public void Setup()
    {
            var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            sut = application.CreateClient();
    }

    #region Basic Api Auth Tests
    
    [TestCase("Mary", "doesnotexist@test.com", "tester456", HttpStatusCode.NotFound)]
    [TestCase("Mary", "mary.rogers@test.com", "tester456", HttpStatusCode.OK)]
    [TestCase("Andy", "andy.peters@test.com", "tester123", HttpStatusCode.OK)]
    public async Task Verify_Authentication_Test(string username, string email, string password, HttpStatusCode expectedStatusCode)
    {
        // Given / When
        var response = await CallAuthAsync(username, email, password);
        
        // Then
        response.StatusCode.Should().Be(expectedStatusCode);
    }
    
    [TestCase("Mary", "mary.rogers@test.com", "tester456")]
    [TestCase("Andy", "andy.peters@test.com", "tester123")]
    public async Task Verify_Generated_Token_Length_Test(string username, string email, string password)
    {
        // Given / When
        var response = await CallAuthAsync(username, email, password);
        var authResponse = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync());
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        authResponse?.Token.Length.Should().Be(333);
    }
    
    [TestCase("Mary", "doesnotexist@test.com", "tester456")]
    [TestCase("Mary", "alsonothere.rogers@test.com", "tester456")]
    [TestCase("Andy", "nope@test.com", "tester123")]
    public async Task Verify_InvalidUser_Returns_404_With_Email_Test(string username, string email, string password)
    {
        // Given / When
        var response = await CallAuthAsync(username, email, password);
        
        var responseContent = await response.Content.ReadAsStringAsync();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Should().Contain(email);
    }
    
    [TestCase("Mary", "mary.rogers@test.com", "guess")]
    [TestCase("Andy", "andy.peters@test.com", "randompass")]
    public async Task Verify_Invalid_Password_Test(string username, string email, string password)
    {
        // Given / When
        var response = await CallAuthAsync(username, email, password);
        
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Endpoints requiring auth tests

    [TestCase("Mary", "mary.rogers@test.com", "tester456")]
    [TestCase("Andy", "andy.peters@test.com", "tester123")]
    public async Task Verify_Get_Vendor_With_Valid_Auth(string username, string email, string password)
    {
        // Given
        var response = await CallAuthAsync("Andy", "andy.peters@test.com", "tester123");
        var token = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync())?.Token;
        sut.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        // When
        var vendors = await sut.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/keyboard/vendor-with-auth"));
        
        // Then
        vendors.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Test]
    public async Task Verify_Get_Vendor_With_Auth_Without_Token()
    {
        // Given / When
        var vendors = await sut.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/keyboard/vendor-with-auth"));
        
        // Then
        vendors.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    #endregion

    private async Task<HttpResponseMessage> CallAuthAsync(string username, string email, string password)
    {
        var user = new User
        {
            Username = username,
            Email = email,
            Password = password
        };

        return await sut.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/login")
        {
            Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json")
        });
    }

    [TearDown]
    public void DeleteResources()
    {
        sut.Dispose();
    }
}