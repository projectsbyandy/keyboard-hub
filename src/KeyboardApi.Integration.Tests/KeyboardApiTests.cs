using System.Text.Json;
using FluentAssertions;
using Keyboard.Common.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace KeyboardApi.Integration.Tests;

public class KeyboardApiTests : WebApplicationFactory<Program>
{
    private HttpClient sut;

    [SetUp]
    public void Setup()
    {
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
        sut = application.CreateClient();
    }

    [Test]
    public async Task Verify_Get_Vendors_Test()
    {
        // Given / When
        var response = await sut.GetAsync("/keyboard/vendors");

        var responseBody = await response.Content.ReadAsStringAsync();
        var vendors = JsonSerializer.Deserialize<IList<Vendor>>(responseBody);
        
        // Then
        vendors?.Count.Should().Be(3, "Number of Vendors");
    }
    
    [TearDown]
    public void DeleteResources()
    {
        sut.Dispose();
    }
}