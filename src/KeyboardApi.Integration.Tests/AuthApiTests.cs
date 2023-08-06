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
    
    [TestCase("Mary", "mary.rogers@test.com", "tester456", HttpStatusCode.OK)]
    [TestCase("Andy", "andy.peters@test.com", "tester123", HttpStatusCode.OK)]
    public async Task Verify_Generated_Token_Length_Test(string username, string email, string password, HttpStatusCode expectedStatusCode)
    {
        // Given / When
        var response = await CallAuthAsync(username, email, password);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        // Then
        response.StatusCode.Should().Be(expectedStatusCode);
        responseContent.Length.Should().Be(335);
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

    private async Task<HttpResponseMessage> CallAuthAsync(string username, string email, string password)
    {
        var user = new User
        {
            Username = username,
            Email = email,
            Password = password
        };

        return await sut.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/login") {
            Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json")
        });
    }
    
    [TearDown]
    public void DeleteResources()
    {
        sut.Dispose();
    }
}