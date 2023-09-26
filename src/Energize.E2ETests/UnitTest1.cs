using System.Text;
using Energize.API;
using Energize.Tests.Common;
using FluentAssertions;
using Microsoft.Playwright;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit.Abstractions;

namespace Energize.E2ETests;

public class ProductionPlanE2ETests : IDisposable
{
    // You need to have the web app running locally for these tests to pass.
    
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IPlaywright _playwright;
    private readonly IBrowser _browser;
    private readonly HttpClient _httpClient;

    public ProductionPlanE2ETests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
        _browser = _playwright.Chromium.LaunchAsync().GetAwaiter().GetResult();
        _httpClient = new HttpClient();
    }

    [Fact]
    public async Task IndexPage_ShouldHaveExpectedTitle()
    {
        // Arrange
        var page = await _browser.NewPageAsync();
        const string url = "http://localhost:8888";

        // Act
        await page.GotoAsync(url);

        // Assert
        var title = await page.TitleAsync();
        title.Should().Be("Swagger UI");
    }

    [Fact]
    public async Task CalculateProductionPlan_ShouldReturnExpectedResult_ForGivenInput()
    {
        // Arrange
        var apiUrl = "http://localhost:8888/productionplan";
        var payloadRequest = PayloadRequestFactory.CreatePayloadRequest3(); // Assume it returns a properly serialized JSON string.

        var expectedPlans = new PayloadReply();
        expectedPlans.Powerplants.AddRange(
            new List<ProductionPlanReply>
            {
                new() { Name = "windpark1", P = 90.0 },
                new() { Name = "windpark2", P = 21.6 },
                new() { Name = "gasfiredbig1", P = 460.0 },
                new() { Name = "gasfiredbig2", P = 338.4 },
                new() { Name = "gasfiredsomewhatsmaller", P = 0.0 },
                new() { Name = "tj1", P = 0.0 }
            });

        // Act
        var json = JsonConvert.SerializeObject(payloadRequest, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
        
        _testOutputHelper.WriteLine("Request Payload: " + json);
        var response = await _httpClient.PostAsync(apiUrl, new StringContent(json, Encoding.UTF8, "application/json"));
        _testOutputHelper.WriteLine("Response Status: " + response.StatusCode); // Log the response status code
        _testOutputHelper.WriteLine("Response Reason: " + response.ReasonPhrase); // Log the response reason phrase

        var responseContent = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine("Response Content: " + responseContent); // Log the response payload
        
        response.EnsureSuccessStatusCode();
        
        var result = JsonConvert.DeserializeObject<PayloadReply>(responseContent, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver()
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedPlans, options => options.WithStrictOrdering());
    }

    public void Dispose()
    {
        _browser.DisposeAsync();
        _playwright.Dispose();
        _httpClient.Dispose();
    }
}