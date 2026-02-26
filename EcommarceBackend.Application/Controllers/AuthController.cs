using EcommearceBackend.Business.Abstractions;
using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommearceBackend.Business.src.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace EcommarceBackend.Application.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class AuthController(IAuthService _authService,IUserService _userService,IJwtManager _jwtManager):ControllerBase
	{
		[HttpPost("login")]
		public async Task<ActionResult<string>> AuthenticateUserAsync([FromBody] UserCredentialsDto userCredentialsDto)
		{
			return Ok(await _authService.AutheticateUser(userCredentialsDto));
		}
	
	   [HttpPost("token")]
		public async Task<ActionResult<string>> RefreshTokenAsync([FromBody] UserRefereshTokenDto token)
		{
			return Ok(await _authService.RefreshToken(token.RefereshToken));
		}
		[HttpGet("profile")]
		[Authorize(Policy = "ProfileOwnerOnly")]
		public async Task<ActionResult<ReadUserDto>> GetUserProfileAsync()
		{
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
			{
				return Unauthorized();
			}

			var user = await _userService.GetUserByIdAsync(userId);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}
		[Authorize(Policy = "ProfileOwnerOnly")]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout(UserRefereshTokenDto request)
		{
			await _jwtManager.RevokeAsync(request.RefereshToken);

			return Ok();
		}
		[HttpPost("verify")]
		public async Task<string> VerifyEmailAsync(string email, string code) { 
		var verify= await _authService.VerifyEmail(email, code);
		return verify;
		}
	}
}
