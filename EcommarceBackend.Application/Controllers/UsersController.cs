using AutoMapper;
using EcommearceBackend.Business.Abstractions;
using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommerceBackend.Domain.src.Common;
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
	public class UsersController : ControllerBase
	{

		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public UsersController(IUserService userService, IMapper mapper)
		{
			_userService = userService;
			_mapper = mapper;
		}
		[HttpPost]
		public async Task<ActionResult<ReadUserDto>> CreateUserAsync([FromBody] CreateUserDto createUserDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var user = await _userService.CreateUserAsync(createUserDto);
			var userDto = _mapper.Map<ReadUserDto>(user);
			return Ok(userDto);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]

		public async Task<ActionResult<IEnumerable<ReadUserDto>>> GetAllUsersAsync([FromQuery] QueryOptions queryOptions)
		{
			var users = await _userService.GetAllUsersAsync(queryOptions);
			return Ok(users);
		}

		[HttpGet("{id}")]
		[Authorize(Policy = "ProfileOwnerOnly")]
		public async Task<ActionResult<ReadUserDto>> GetUserByIdAsync(int id)
		{
			var requestingUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (requestingUserIdClaim == null || !int.TryParse(requestingUserIdClaim.Value, out var requestingUserId))
			{
				return Forbid();
			}
			if (requestingUserId == id)
			{
				var user = await _userService.GetUserByIdAsync(id);
				if (user == null)
				{
					return NotFound();
				}
				return Ok(user);
			}
			return Forbid();
		}
		[HttpGet("Email/{email}")]
		[Authorize(Roles = "Admin")]

		public async Task<ActionResult<ReadUserDto>> GetUserByEmailAsnc(string email)
		{
			var user = await _userService.GetUserByEmailAsync(email);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}
		[HttpDelete("{id}")]
		[Authorize(Policy = "ProfileOwnerOnly")]
		public async Task<ActionResult<bool>> DeleteUserByIdAsync(int id)
		{
			var requestingUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

			if (requestingUserIdClaim == null || !int.TryParse(requestingUserIdClaim.Value, out var requestingUserId))
			{
				return Forbid();
			}
			if (User.IsInRole("Admin") || requestingUserId == id)
			{
				var result = await _userService.DeleteUserByIdAsync(id);
				if (!result)
				{
					return NotFound();
				}
				return Ok(result);
			}
			return Forbid();


		}

		[HttpPut("{id}")]
		[Authorize (Policy= "ProfileOwnerOnly")]
		public async Task<ActionResult<ReadUserDto>> UpdateUserAsync(int id, [FromBody] UpdateUserDto updateUserDto)
		{
			var requestingUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
			if (requestingUserIdClaim == null || !int.TryParse(requestingUserIdClaim.Value, out var requestingUserId))
			{
				return Forbid();
			}
			if (User.IsInRole("Admin") || requestingUserId == id)
			{
				var updatedUser = await _userService.UpdateUserAsync(id, updateUserDto);
				if (updatedUser == null)
				{
					return NotFound();
				}
				var readUserDto = _mapper.Map<ReadUserDto>(updatedUser);
				return Ok(readUserDto);
			}
			return Forbid();
		}
		[HttpPost("Admin/")]
		[Authorize(Roles = "Admin")]

		public async Task<ActionResult<ReadUserDto>> CreateAdminAsync([FromBody] CreateUserDto userDto)
		{
			var adminUser = await _userService.CreateAdminAsync(userDto);
			return Ok(adminUser);
		}
	}
}
