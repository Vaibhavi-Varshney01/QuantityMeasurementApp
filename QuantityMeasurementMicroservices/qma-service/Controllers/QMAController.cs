using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QMAService.Interface;
using SharedModels.DTO;

namespace QMAService.Controllers;

[ApiController]
[Route("api/v1/quantities")]
[Authorize]
public class QMAController : ControllerBase
{
    private readonly IQMAService _svc;
    public QMAController(IQMAService svc) => _svc = svc;

    [HttpPost("compare")]
    public async Task<IActionResult> Compare([FromBody] QuantityInputDTO input)
    {
        try { return Ok(await _svc.Compare(input.ThisQuantityDTO!, input.ThatQuantityDTO!)); }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] QuantityInputDTO input)
    {
        try { return Ok(await _svc.Add(input.ThisQuantityDTO!, input.ThatQuantityDTO!)); }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("subtract")]
    public async Task<IActionResult> Subtract([FromBody] QuantityInputDTO input)
    {
        try { return Ok(await _svc.Subtract(input.ThisQuantityDTO!, input.ThatQuantityDTO!)); }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("divide")]
    public async Task<IActionResult> Divide([FromBody] QuantityInputDTO input)
    {
        try { return Ok(await _svc.Divide(input.ThisQuantityDTO!, input.ThatQuantityDTO!)); }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
    }

    [HttpPost("convert")]
    public async Task<IActionResult> Convert([FromBody] QuantityInputDTO input)
    {
        try { return Ok(await _svc.Convert(input.ThisQuantityDTO!, input.ThatQuantityDTO!.Unit!)); }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpGet("history/operation/{operation}")]
    public async Task<IActionResult> GetByOp(string operation) => Ok(await _svc.GetHistoryByOperation(operation));

    [HttpGet("history/type/{type}")]
    public async Task<IActionResult> GetByType(string type) => Ok(await _svc.GetHistoryByType(type));

    [HttpGet("history/errored")]
    public async Task<IActionResult> GetErrors() => Ok(await _svc.GetErrorHistory());

    [HttpGet("count/{operation}")]
    public async Task<IActionResult> Count(string operation) => Ok(await _svc.CountByOperation(operation));
}
