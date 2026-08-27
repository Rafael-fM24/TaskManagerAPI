using Application.DTOs.User;
using Application.Interfaces.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _tokenService = tokenService ??  throw new ArgumentNullException(nameof(tokenService));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO dto)
    {
        await _userService.RegisterAsync(dto);

        return Created();
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDTO dto)
    {
        var user = await _userService.AuthenticateAsync(dto);

        if (user is null)
            return Unauthorized(new
            {
                message = "Invalid email or password"
            });


        var token = _tokenService.GenerateToken(user);

        return Ok(new
        {
            token
        });
    }
}