using EcommearceBackend.Business.Abstractions;
using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommearceBackend.Business.src.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommarceBackend.Application.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class AuthController(IAuthService _authService,IUserService _userService):ControllerBase
	{
		[HttpPost("login")]
		public async Task<ActionResult<string>> AuthenticateUserAsync([FromBody] UserCredentialsDto userCredentialsDto)
		{
			return Ok(await _authService.AutheticateUser(userCredentialsDto));
		}
	}
}
