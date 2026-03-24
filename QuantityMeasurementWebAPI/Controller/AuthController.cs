using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementModel.DTO;
using QuantityMeasurementBusinessLayer.Interface;

namespace QuantityMeasurementWebAPI.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
        => _authService = authService;

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDTO dto)
    {
        try
        {
            var result = _authService.Register(dto);
            return StatusCode(201, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO dto)
    {
        try
        {
            var result = _authService.Login(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
 
