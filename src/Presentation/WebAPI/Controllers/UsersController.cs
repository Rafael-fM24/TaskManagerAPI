using Application.DTOs.User;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var user = await _userService.GetCurrentUserAsync();

        if (user == null)
            return NotFound();

        return Ok(user);
    }
    
    [HttpPut("me")]
    public async Task<IActionResult> UpdateAsync(UpdateUserDTO dto)
    {
        await _userService.Update(dto);
        
        return NoContent();
    }
    
    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordDTO dto)
    {
        await _userService.ChangePasswordAsync(dto);

        return NoContent();
    }

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteAsync()
    {
        await _userService.Delete();
        
        return NoContent();
    }
}