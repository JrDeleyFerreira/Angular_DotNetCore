using Microsoft.AspNetCore.Identity;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces;

public interface IAccountService
{
	Task<bool> UserExists(string username);
	Task<UserUpdateDto?> GetUserByUserNameAsync(string username);
	Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password);
	Task<UserUpdateDto?> CreateAccountAsync(UserDto userDto);
	Task<UserUpdateDto?> UpdateAccount(UserUpdateDto userUpdateDto);
}
