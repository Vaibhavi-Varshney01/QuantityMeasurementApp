using AuthService.Interface;
using Microsoft.AspNetCore.Mvc;
using SharedModels.DTO;

namespace AuthService.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _svc;
    public AuthController(IAuthService svc) => _svc = svc;

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDTO dto)
    {
        try { return StatusCode(201, _svc.Register(dto)); }
        catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO dto)
    {
        try { return Ok(_svc.Login(dto)); }
        catch (Exception ex) { return Unauthorized(new { message = ex.Message }); }
    }
}
