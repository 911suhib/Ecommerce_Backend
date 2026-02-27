using EcommearceBackend.Business.src.Dtos.UserDtos;
using EcommerceBackend.Domain.src.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.Abstractions
{
	public interface IUserService
	{
		Task<IEnumerable<ReadUserDto>> GetAllUsersAsync(QueryOptions queryOptions);
  		Task<ReadUserDto> GetUserByIdAsync(int userId);
		Task<ReadUserDto> GetUserByEmailAsync(string email);
		Task<ReadUserDto> UpdateUserAsync(int userId, UpdateUserDto userDto);
		Task<bool> DeleteUserByIdAsync(int userId);
	}
}
