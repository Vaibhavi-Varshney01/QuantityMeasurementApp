using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementBusinessLayer.DTO;

namespace QuantityMeasurementWebAPI.Controllers;

[ApiController]
[Route("api/v1/quantities")]
[Authorize]
public class QuantityMeasurementController : ControllerBase
{
    private readonly IQuantityMeasurementService _service;

    public QuantityMeasurementController(IQuantityMeasurementService service)
        => _service = service;

    [HttpPost("compare")]
    public IActionResult Compare([FromBody] QuantityInputDTO input)
    {
        try
        {
            var result = _service.Compare(input.ThisQuantityDTO!, input.ThatQuantityDTO!);
            return Ok(result);
        }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("add")]
    public IActionResult Add([FromBody] QuantityInputDTO input)
    {
        try
        {
            var result = _service.Add(input.ThisQuantityDTO!, input.ThatQuantityDTO!);
            return Ok(result);
        }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("subtract")]
    public IActionResult Subtract([FromBody] QuantityInputDTO input)
    {
        try
        {
            var result = _service.Subtract(input.ThisQuantityDTO!, input.ThatQuantityDTO!);
            return Ok(result);
        }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("divide")]
    public IActionResult Divide([FromBody] QuantityInputDTO input)
    {
        try
        {
            var result = _service.Divide(input.ThisQuantityDTO!, input.ThatQuantityDTO!);
            return Ok(result);
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
    }

    [HttpPost("convert")]
    public IActionResult Convert([FromBody] QuantityInputDTO input)
    {
        try
        {
            var result = _service.Convert(
                input.ThisQuantityDTO!, input.ThatQuantityDTO!.Unit!);
            return Ok(result);
        }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpGet("history/operation/{operation}")]
    public IActionResult GetByOperation(string operation)
        => Ok(_service.GetHistoryByOperation(operation));

    [HttpGet("history/type/{type}")]
    public IActionResult GetByType(string type)
        => Ok(_service.GetHistoryByType(type));

    [HttpGet("history/errored")]
    public IActionResult GetErrors()
        => Ok(_service.GetErrorHistory());

    [HttpGet("count/{operation}")]
    public IActionResult Count(string operation)
        => Ok(_service.CountByOperation(operation));
}
