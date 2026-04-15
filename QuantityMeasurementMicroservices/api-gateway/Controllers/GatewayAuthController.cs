using Microsoft.AspNetCore.Mvc;
using SharedModels.DTO;
using System.Net.Http.Json;

namespace ApiGateway.Controllers;

/// <summary>
/// Proxies auth requests to auth-service.
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public class GatewayAuthController : ControllerBase
{
    private readonly IHttpClientFactory _factory;
    public GatewayAuthController(IHttpClientFactory f) => _factory = f;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var client = _factory.CreateClient("auth");
        var resp   = await client.PostAsJsonAsync("api/v1/auth/register", dto);
        var body   = await resp.Content.ReadFromJsonAsync<object>();
        return StatusCode((int)resp.StatusCode, body);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var client = _factory.CreateClient("auth");
        var resp   = await client.PostAsJsonAsync("api/v1/auth/login", dto);
        var body   = await resp.Content.ReadFromJsonAsync<object>();
        return StatusCode((int)resp.StatusCode, body);
    }
}
