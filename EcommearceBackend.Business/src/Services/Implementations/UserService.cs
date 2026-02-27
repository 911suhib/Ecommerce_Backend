using AutoMapper;
using EcommearceBackend.Business.Abstractions;
using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommearceBackend.Business.src.Services.Abstractions;
using EcommearceBackend.Business.src.Services.Common;
using EcommerceBackend.Domain.Entities;
using EcommerceBackend.Domain.src.Abstractions;
using EcommerceBackend.Domain.src.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Implementations
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly ISanitizerService _sanitizerService;
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;
		public UserService(IUserRepository userRepository, ISanitizerService sanitizerService, IMapper mapper, IEmailService emailService)
		{
			_userRepository = userRepository;
			_sanitizerService = sanitizerService;
			_mapper = mapper;
			_emailService = emailService;
		}
		 
		

		 
		public async Task<bool> DeleteUserByIdAsync(int userId)
		{
			var user = await _userRepository.GetByIdAsync(userId);
			if (user == null)
			{
				return false;
			}
			return await _userRepository.DeleteByIdAsync(userId);
		}

		public async Task<IEnumerable<ReadUserDto>> GetAllUsersAsync(QueryOptions queryOptions)
		{
			var users = await _userRepository.GetAllAsync(queryOptions);
			var readUserDtos = _mapper.Map<IEnumerable<ReadUserDto>>(users);
			return readUserDtos;
		}

		public async Task<ReadUserDto> GetUserByEmailAsync(string email)
		{
			var user = await _userRepository.GetUserByEmailAsync(email);
			var readUserDto = _mapper.Map<ReadUserDto>(user);
			return readUserDto;
		}

		public async Task<ReadUserDto> GetUserByIdAsync(int userId)
		{
			var user = await _userRepository.GetByIdAsync(userId);
			if (user == null)
			{
				return null;
			}
			var readUserDto = _mapper.Map<ReadUserDto>(user);
			return readUserDto;

		}

		public async Task<ReadUserDto> UpdateUserAsync(int userId, UpdateUserDto userDto)
		{
			try
			{
				var existingUser = await _userRepository.GetByIdAsync(userId);

				if (existingUser == null)
				{
					throw new ArgumentException($"User with ID {userId} not found.");
				}

				var userDtoProperties = typeof(UpdateUserDto).GetProperties();
				foreach (var property in userDtoProperties)
				{
					var dtoValue = property.GetValue(userDto);
					if (dtoValue is string strValue && string.IsNullOrWhiteSpace(strValue))
					{
						throw new ArgumentException($"{property.Name} cannot be empty or whitespace.");
					}
					else
					{
						var userProperty = existingUser.GetType().GetProperty(property.Name);
						userProperty.SetValue(existingUser, dtoValue);
					}
				}
				existingUser = await _userRepository.UpdateAsync(userId, existingUser);
				var readUserDto = _mapper.Map<ReadUserDto>(existingUser);
				return readUserDto;
			}
			catch (Exception ex)
			{
				
					Console.WriteLine("Error: " + ex.Message);

					if (ex.InnerException != null)
					{
						Console.WriteLine("Inner exception: " + ex.InnerException.Message);
					}
					throw;
				
			}
		}
	}
}
