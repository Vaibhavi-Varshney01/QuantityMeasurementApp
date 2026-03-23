using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using QuantityMeasurementRepository;

namespace QuantityMeasurementTests;

[TestFixture]
[NonParallelizable]
public class UC17_WebApiTests
{
    private AuthenticatedWebApiFactory _authenticatedFactory = null!;
    private HttpClient _authenticatedClient = null!;

    private UnauthenticatedWebApiFactory _unauthenticatedFactory = null!;
    private HttpClient _unauthenticatedClient = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _authenticatedFactory = new AuthenticatedWebApiFactory();
        _authenticatedClient = _authenticatedFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        _unauthenticatedFactory = new UnauthenticatedWebApiFactory();
        _unauthenticatedClient = _unauthenticatedFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _authenticatedClient.Dispose();
        _authenticatedFactory.Dispose();
        _unauthenticatedClient.Dispose();
        _unauthenticatedFactory.Dispose();
    }

    [Test]
    public async Task testSpringBootApplicationStarts()
    {
        var response = await _authenticatedClient.GetAsync("/");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
        Assert.That(response.Headers.Location, Is.Not.Null);
        Assert.That(response.Headers.Location!.ToString(), Is.EqualTo("/swagger"));
    }

    [Test]
    public async Task testSwaggerUILoads()
    {
        var response = await _authenticatedClient.GetAsync("/swagger/index.html");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var contentType = response.Content.Headers.ContentType?.MediaType;
        Assert.That(contentType, Is.EqualTo("text/html"));

        var html = await response.Content.ReadAsStringAsync();
        StringAssert.Contains("Swagger UI", html);
    }

    [Test]
    public async Task testOpenAPIDocumentation()
    {
        var response = await _authenticatedClient.GetAsync("/swagger/v1/swagger.json");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        Assert.That(doc.RootElement.TryGetProperty("paths", out var paths), Is.True);
        Assert.That(paths.TryGetProperty("/api/v1/quantities/compare", out _), Is.True);
        Assert.That(paths.TryGetProperty("/api/v1/quantities/add", out _), Is.True);
        Assert.That(paths.TryGetProperty("/api/v1/quantities/convert", out _), Is.True);
    }

    [Test]
    public async Task testActuatorHealthEndpoint()
    {
        var response = await _authenticatedClient.GetAsync("/actuator/health");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var body = await response.Content.ReadAsStringAsync();
        Assert.That(body, Does.Contain("Healthy").Or.Contain("healthy").Or.Contain("UP"));
    }

    [Test]
    public async Task testRESTEndpointSecurity_Unauthorized()
    {
        var payload = new
        {
            thisQuantityDTO = new { value = 1.0, unit = "METER", measurementType = "Length", operationType = "COMPARE" },
            thatQuantityDTO = new { value = 1.0, unit = "METER", measurementType = "Length", operationType = "COMPARE" }
        };

        var response = await _unauthenticatedClient.PostAsJsonAsync("/api/v1/quantities/compare", payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task testRestEndpointCompareQuantities()
    {
        var payload = new
        {
            thisQuantityDTO = new { value = 1.0, unit = "METER", measurementType = "Length", operationType = "COMPARE" },
            thatQuantityDTO = new { value = 1.0, unit = "METER", measurementType = "Length", operationType = "COMPARE" }
        };

        var response = await _authenticatedClient.PostAsJsonAsync("/api/v1/quantities/compare", payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        Assert.That(doc.RootElement.GetProperty("operation").GetString(), Is.EqualTo("COMPARE"));
        Assert.That(doc.RootElement.GetProperty("resultString").GetString(), Is.EqualTo("True"));
    }

    [Test]
    public async Task testRestEndpointAddQuantities()
    {
        var payload = new
        {
            thisQuantityDTO = new { value = 1.0, unit = "FEET", measurementType = "Length", operationType = "ADD" },
            thatQuantityDTO = new { value = 1.0, unit = "FEET", measurementType = "Length", operationType = "ADD" }
        };

        var response = await _authenticatedClient.PostAsJsonAsync("/api/v1/quantities/add", payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        Assert.That(doc.RootElement.GetProperty("operation").GetString(), Is.EqualTo("ADD"));
        Assert.That(doc.RootElement.GetProperty("resultValue").GetDouble(), Is.EqualTo(2.0).Within(1e-9));
        Assert.That(doc.RootElement.GetProperty("resultUnit").GetString(), Is.EqualTo("FEET"));
    }

    [Test]
    public async Task testRestEndpointConvertQuantities()
    {
        var payload = new
        {
            thisQuantityDTO = new { value = 1.0, unit = "METER", measurementType = "Length", operationType = "CONVERT" },
            thatQuantityDTO = new { value = 0.0, unit = "FEET", measurementType = "Length", operationType = "CONVERT" }
        };

        var response = await _authenticatedClient.PostAsJsonAsync("/api/v1/quantities/convert", payload);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        Assert.That(doc.RootElement.GetProperty("operation").GetString(), Is.EqualTo("CONVERT"));
        Assert.That(doc.RootElement.GetProperty("resultString").GetString(), Is.EqualTo("1 FEET"));
    }

    [Test]
    public async Task testRestEndpointInvalidInput_Returns400()
    {
        var response = await _authenticatedClient.PostAsync(
            "/api/v1/quantities/add",
            new StringContent("{", Encoding.UTF8, "application/json"));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task testIntegrationTest_MultipleOperations()
    {
        var comparePayload = new
        {
            thisQuantityDTO = new { value = 5.0, unit = "FEET", measurementType = "Length", operationType = "COMPARE" },
            thatQuantityDTO = new { value = 5.0, unit = "FEET", measurementType = "Length", operationType = "COMPARE" }
        };
        var addPayload = new
        {
            thisQuantityDTO = new { value = 3.0, unit = "FEET", measurementType = "Length", operationType = "ADD" },
            thatQuantityDTO = new { value = 4.0, unit = "FEET", measurementType = "Length", operationType = "ADD" }
        };

        var compareResponse = await _authenticatedClient.PostAsJsonAsync("/api/v1/quantities/compare", comparePayload);
        Assert.That(compareResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var addResponse = await _authenticatedClient.PostAsJsonAsync("/api/v1/quantities/add", addPayload);
        Assert.That(addResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var historyResponse = await _authenticatedClient.GetAsync("/api/v1/quantities/history/operation/COMPARE");
        Assert.That(historyResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var historyJson = await historyResponse.Content.ReadAsStringAsync();
        using var historyDoc = JsonDocument.Parse(historyJson);
        Assert.That(historyDoc.RootElement.ValueKind, Is.EqualTo(JsonValueKind.Array));
        Assert.That(historyDoc.RootElement.GetArrayLength(), Is.GreaterThanOrEqualTo(1));
    }

    private sealed class UnauthenticatedWebApiFactory : WebApplicationFactory<Program>
    {
        private readonly string _databaseName = $"qm_test_{Guid.NewGuid():N}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Redis"] = ""
                });
            });

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<QuantityMeasurementDbContext>));
                services.RemoveAll(typeof(QuantityMeasurementDbContext));

                services.AddDbContext<QuantityMeasurementDbContext>(options =>
                    options.UseInMemoryDatabase(_databaseName));

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }

    private sealed class AuthenticatedWebApiFactory : WebApplicationFactory<Program>
    {
        private readonly string _databaseName = $"qm_test_{Guid.NewGuid():N}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Redis"] = ""
                });
            });

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<QuantityMeasurementDbContext>));
                services.RemoveAll(typeof(QuantityMeasurementDbContext));

                services.AddDbContext<QuantityMeasurementDbContext>(options =>
                    options.UseInMemoryDatabase(_databaseName));

                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }

    private sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "test-user"),
                    new Claim(ClaimTypes.Name, "test-user")
                },
                authenticationType: "Test");

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
