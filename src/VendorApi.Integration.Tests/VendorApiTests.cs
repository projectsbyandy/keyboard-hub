using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using VendorApi.Integration.Tests.Setup;
using VendorApi.Models.Auth;
using VendorApi.Models.Config;
using VendorApi.Models.Vendor;

namespace VendorApi.Integration.Tests;

internal class VendorApiTests : IntegrationBase
{
    private HttpClient _client;
    private static readonly SemaphoreSlim Semaphore = new(1);
    private const int SemaphoreMaxWait = 1000;

    [SetUp]
    public async Task SetUp()
    {
        _client = GetClient();
        await SetupAuthAsync();
    }

    [TestCase("doesnotexist")]
    [TestCase("NA")]
    [TestCase("BaH")]
    public async Task Return_404_On_a_non_existent_vendor_endpoint(string endpointName)
    {
        // Arrange / Act
        var response = await _client.GetAsync($"api/vendor/{endpointName}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Get_Vendors_Returns_All_VendorDetails_Successfully()
    {
        // Arrange / Act
        var response = await _client.GetAsync("api/vendor/all");
        response.EnsureSuccessStatusCode();

        var raw = Guard.Against.NullOrEmpty(await response.Content.ReadAsStringAsync());
        var vendors = JsonSerializer.Deserialize<IList<VendorDetails>>(raw);
        
        // Assert
        vendors?.Count.Should().Be(3);
        vendors?.Any(vendor => vendor.Id == new Guid("035bb0ee-bd4b-40b5-b459-24e33e1647c0") &&
                               vendor is { Name: "Prototypist", IsLive: true, YearsActive: 12 }).Should().BeTrue();
    }
    
    [TestCaseSource(nameof(ExpectedVendors))]
    public async Task Get_Vendor_Returns_VendorDetails_Successfully(VendorDetails expectedVendor)
    {
        // Arrange / Act
        var response = await _client.GetAsync($"api/vendor/{expectedVendor.Id}");
        response.EnsureSuccessStatusCode();

        var raw = Guard.Against.NullOrEmpty(await response.Content.ReadAsStringAsync());
        var actualVendor = JsonSerializer.Deserialize<VendorDetails>(raw);
        
        // Assert
        actualVendor.Should().Be(expectedVendor);
    }
    
    private static IEnumerable<VendorDetails> ExpectedVendors
    {
        get
        {
            yield return new VendorDetails() { Id = new Guid("035bb0ee-bd4b-40b5-b459-24e33e1647c0"), Name = "Prototypist", IsLive = true, YearsActive = 12 };
            yield return new VendorDetails() { Id = new Guid("8e4dab9f-8bfa-4ba5-8048-5efc7bd06fa1"), Name = "MechUk", IsLive = false, YearsActive = 3 };
        }
    }
    
    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }

    private async Task SetupAuthAsync()
    {
        await Semaphore.WaitAsync(SemaphoreMaxWait);
        var user = WebApplicationFactory.Services.GetService<SeedUsers>()?.First();
        Guard.Against.Null(user);
        
        try
        {
            var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/authenticate/login")
            {
                Content = new StringContent(JsonSerializer.Serialize(new User
                {
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password
                }), Encoding.UTF8, "application/json")
            });
            
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadAsStringAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        finally
        {
            Semaphore.Release();
        }
    }
}