using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels.DTO;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ApiGateway.Controllers;

/// <summary>
/// Proxies all quantity operations to qma-service, forwarding the JWT.
/// </summary>
[ApiController]
[Route("api/v1/quantities")]
[Authorize]
public class GatewayQMAController : ControllerBase
{
    private readonly IHttpClientFactory _factory;
    public GatewayQMAController(IHttpClientFactory f) => _factory = f;

    private HttpClient QMAClient()
    {
        var client = _factory.CreateClient("qma");
        if (Request.Headers.TryGetValue("Authorization", out var auth))
            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse(auth.ToString());
        return client;
    }

    [HttpPost("compare")]
    public async Task<IActionResult> Compare([FromBody] QuantityInputDTO dto)
    {
        var r = await QMAClient().PostAsJsonAsync("api/v1/quantities/compare", dto);
        return StatusCode((int)r.StatusCode, await r.Content.ReadFromJsonAsync<object>());
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] QuantityInputDTO dto)
    {
        var r = await QMAClient().PostAsJsonAsync("api/v1/quantities/add", dto);
        return StatusCode((int)r.StatusCode, await r.Content.ReadFromJsonAsync<object>());
    }

    [HttpPost("subtract")]
    public async Task<IActionResult> Subtract([FromBody] QuantityInputDTO dto)
    {
        var r = await QMAClient().PostAsJsonAsync("api/v1/quantities/subtract", dto);
        return StatusCode((int)r.StatusCode, await r.Content.ReadFromJsonAsync<object>());
    }

    [HttpPost("divide")]
    public async Task<IActionResult> Divide([FromBody] QuantityInputDTO dto)
    {
        var r = await QMAClient().PostAsJsonAsync("api/v1/quantities/divide", dto);
        return StatusCode((int)r.StatusCode, await r.Content.ReadFromJsonAsync<object>());
    }

    [HttpPost("convert")]
    public async Task<IActionResult> Convert([FromBody] QuantityInputDTO dto)
    {
        var r = await QMAClient().PostAsJsonAsync("api/v1/quantities/convert", dto);
        return StatusCode((int)r.StatusCode, await r.Content.ReadFromJsonAsync<object>());
    }

    [HttpGet("history/operation/{operation}")]
    public async Task<IActionResult> GetByOp(string operation)
    {
        var r = await QMAClient().GetAsync($"api/v1/quantities/history/operation/{operation}");
        return StatusCode((int)r.StatusCode, await r.Content.ReadFromJsonAsync<object>());
    }

    [HttpGet("history/type/{type}")]
    public async Task<IActionResult> GetByType(string type)
    {
        var r = await QMAClient().GetAsync($"api/v1/quantities/history/type/{type}");
        return StatusCode((int)r.StatusCode, await r.Content.ReadFromJsonAsync<object>());
    }

    [HttpGet("history/errored")]
    public async Task<IActionResult> GetErrors()
    {
        var r = await QMAClient().GetAsync("api/v1/quantities/history/errored");
        return StatusCode((int)r.StatusCode, await r.Content.ReadFromJsonAsync<object>());
    }

    [HttpGet("count/{operation}")]
    public async Task<IActionResult> Count(string operation)
    {
        var r = await QMAClient().GetAsync($"api/v1/quantities/count/{operation}");
        return StatusCode((int)r.StatusCode, await r.Content.ReadFromJsonAsync<object>());
    }
}
