using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementBusinessLayer;
using QuantityMeasurementModel.DTO;

namespace QuantityMeasurementWebAPI.Controllers;

[ApiController]
[Route("api/v1/quantities")]
public class QuantityMeasurementController : ControllerBase
{
    private readonly IQuantityMeasurementService _service;

    public QuantityMeasurementController(IQuantityMeasurementService service)
        => _service = service;

    [AllowAnonymous]
    [HttpPost("compare")]
    public IActionResult Compare([FromBody] QuantityInputDTO input)
    {
        try
        {
            if (input == null || input.ThisQuantityDTO == null || input.ThatQuantityDTO == null)
                return BadRequest(new { message = "Invalid input" });

            var result = _service.Compare(input.ThisQuantityDTO, input.ThatQuantityDTO);
            return Ok(result);
        }
        catch (Exception ex)
        {
            var message = ex.InnerException != null 
                ? $"{ex.Message} --> {ex.InnerException.Message}" 
                : ex.Message;
            return BadRequest(new { message });
        }
    }

    [AllowAnonymous]
    [HttpPost("add")]
    public IActionResult Add([FromBody] QuantityInputDTO input)
    {
        try
        {
            var result = _service.Add(input.ThisQuantityDTO!, input.ThatQuantityDTO!);
            return Ok(result);
        }
        catch (Exception ex)
        {
            var message = ex.InnerException != null 
                ? $"{ex.Message} --> {ex.InnerException.Message}" 
                : ex.Message;
            return BadRequest(new { message });
        }
    }

    [AllowAnonymous]
    [HttpPost("subtract")]
    public IActionResult Subtract([FromBody] QuantityInputDTO input)
    {
        try
        {
            var result = _service.Subtract(input.ThisQuantityDTO!, input.ThatQuantityDTO!);
            return Ok(result);
        }
        catch (Exception ex)
        {
            var message = ex.InnerException != null 
                ? $"{ex.Message} --> {ex.InnerException.Message}" 
                : ex.Message;
            return BadRequest(new { message });
        }
    }

    [AllowAnonymous]
    [HttpPost("divide")]
    public IActionResult Divide([FromBody] QuantityInputDTO input)
    {
        try
        {
            var result = _service.Divide(input.ThisQuantityDTO!, input.ThatQuantityDTO!);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("convert")]
    public IActionResult Convert([FromBody] QuantityInputDTO input)
    {
        try
        {
            var result = _service.Convert(
                input.ThisQuantityDTO!, input.ThatQuantityDTO!.Unit!);

            return Ok(result);
        }
        catch (Exception ex)
        {
            var message = ex.InnerException != null 
                ? $"{ex.Message} --> {ex.InnerException.Message}" 
                : ex.Message;
            return BadRequest(new { message });
        }
    }

    [Authorize]
    [HttpGet("history")]
    public IActionResult GetHistory()
    {
        try
        {
            return Ok(_service.GetAllHistory());
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("history/errors")]
    public IActionResult GetErrors()
    {
        try
        {
            return Ok(_service.GetErrorHistory());
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("counts")]
    public IActionResult GetCounts()
    {
        try
        {
            var ops = new[] { "COMPARE", "ADD", "SUBTRACT", "DIVIDE", "CONVERT" };
            return Ok(ops.Select(op => _service.CountByOperation(op)).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}