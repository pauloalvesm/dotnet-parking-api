using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parking.Service.DTOs.Account;
using Parking.Service.Services.Interfaces.Account;

namespace Parking.API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenService _tokenService;

    public UserController(UserManager<IdentityUser> userManager, 
                          SignInManager<IdentityUser> signInManager, 
                          ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] UserDTO userDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new IdentityUser
        {
            UserName = userDTO.Email,
            Email = userDTO.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, userDTO.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { Message = "User registered successfully." });
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(loginDTO.Email);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found.");
            return BadRequest(ModelState);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            UserTokenDTO token = _tokenService.GenerateToken(loginDTO.Email);
            return Ok(token);
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid Login");
            return BadRequest(ModelState);
        }
    }
}
