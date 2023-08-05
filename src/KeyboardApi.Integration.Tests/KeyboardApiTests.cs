using System.Text.Json;
using FluentAssertions;
using KeyboardApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace KeyboardApi.Integration.Tests;

public class KeyboardApiTests
{
    private HttpClient sut;
    [SetUp]
    public void Setup()
    {
        var factory = new WebApplicationFactory<Program>();
        sut = factory.CreateClient();
    }

    [Test]
    public async Task GetVendorsTest()
    {
        var response = await sut.GetAsync("/keyboard/vendors");

        var vendors = JsonSerializer.Deserialize<IList<Vendor>>(await response.Content.ReadAsStringAsync());

        vendors?.Count.Should().Be(3, "Number of Vendors");
    }
}