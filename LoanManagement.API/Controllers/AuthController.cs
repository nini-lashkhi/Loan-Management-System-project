using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;

    private static readonly Dictionary<string, string> _users = new()
    {
        { "admin", "admin123" }
    };

    public AuthController(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    /// <summary>Login - JWT Token-ის მიღება</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        if (!_users.TryGetValue(dto.Username, out var password) || password != dto.Password)
            return Unauthorized(new { error = "არასწორი მონაცემები." });

        var token = _jwtService.GenerateToken(dto.Username);
        return Ok(new TokenResponseDto(token));
    }

    /// <summary>რეგისტრაცია</summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        if (_users.ContainsKey(dto.Username))
            return BadRequest(new { error = "მომხმარებელი უკვე არსებობს." });

        _users[dto.Username] = dto.Password;
        var token = _jwtService.GenerateToken(dto.Username);
        return Ok(new TokenResponseDto(token));
    }
}