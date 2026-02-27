using AutoMapper;
using EcommearceBackend.Business.Abstractions;
using EcommearceBackend.Business.src.Dtos.PasswordChange;
using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommerceBackend.Domain.src.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

 
namespace EcommarceBackend.Application.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
 
	public class AuthController:ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IUserService _userService;
		private readonly IJwtManager _jwtManager;
		private readonly IMapper _mapper;

		public AuthController(IAuthService authService, IUserService userService, IJwtManager jwtManager, IMapper mapper)
		{
			_authService = authService;
			_userService = userService;
			_jwtManager = jwtManager;
			_mapper = mapper;
		}


		[HttpPost("login")]
		[EnableRateLimiting("OtpPolicy")]


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
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
				return Unauthorized();

			await _jwtManager.RevokeAsync(userId);
			return Ok();
		}
		[HttpPost("verify")]
		[EnableRateLimiting("OtpPolicy")]

		public async Task<ActionResult<string>> VerifyEmailAsync([FromQuery] string email, [FromQuery] string code)
		{
			var verify = await _authService.VerifyEmail(email, code);
			return verify;
		}
		[HttpPost("Regester")]
		public async Task<ActionResult<ReadUserDto>> CreateUserAsync([FromBody] CreateUserDto createUserDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var user = await _authService.CreateUserAsync(createUserDto);
			var userDto = _mapper.Map<ReadUserDto>(user);
			return Ok(userDto);
		}
		[HttpPost("Register")]
		public async Task<ActionResult<ReadUserDto>> CreateAdminAsync([FromBody] CreateUserDto userDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var adminUser = await _authService.CreateAdminAsync(userDto);
			var user = _mapper.Map<ReadUserDto>(adminUser);
			return Ok(user);

		}
		[HttpPost("forget-password")]
		[EnableRateLimiting("OtpPolicy")]
		public async Task<IActionResult> ForgetPassword([FromBody] EmailDto request)
		{
			if (request == null || string.IsNullOrWhiteSpace(request.Email))
				return BadRequest(new { message = "Email is required" });
			try
			{
				await _authService.ForgetBassword(request.Email);
				return Ok(new { messae = "Verification code sent tto your email" });
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		[HttpPost("check-code")]

		public async Task<IActionResult> CheckCode([FromBody] VerifyCodeDto request)
		{
			try
			{
				bool isValid = await _authService.CheckCodeHashed(request.Email, request.Code);
				return Ok(new { valid = isValid, message = "Code is valid" });
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { valid = false, message = ex.Message });
			}
		}
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
		{
			try
			{
				var user = await _authService.ResetPasswordHashed(request.Email, request.Code, request.NewPassword);


				return Ok(new { success = true, message = "Password changed successfully" });
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { success = false, message = ex.Message });
			}
		}
	}

}
