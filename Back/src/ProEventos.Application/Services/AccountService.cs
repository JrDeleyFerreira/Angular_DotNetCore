using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Persisttence.Interfaces;

namespace ProEventos.Application.Services;

public class AccountService : IAccountService
{
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly IMapper _mapper;
	private readonly IUserPersistence _userPersistence;

	public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUserPersistence userPersistence)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_mapper = mapper;
		_userPersistence = userPersistence;
	}

	public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
	{
		try
		{
			//var user = await _userManager.Users.SingleOrDefaultAsync(
			//	us => string.Equals(us.UserName!,userUpdateDto.UserName!, StringComparison.CurrentCultureIgnoreCase));

			var user = await _userManager.Users.SingleOrDefaultAsync(
				us => us.UserName!.ToLower().Equals(userUpdateDto.UserName!.ToLower()));

			return await _signInManager.CheckPasswordSignInAsync(user!, password, false);
		}
		catch (Exception e)
		{
			throw new Exception($"Erro ao tentar verificar o password! Erro: {e.Message}");
		}
	}

	public async Task<UserUpdateDto?> CreateAccountAsync(UserDto userDto)
	{
		try
		{
			var user = _mapper.Map<User>(userDto);
			var result = await _userManager.CreateAsync(user, userDto.Password!);

			if (result.Succeeded)
			{
				var userToReturn = _mapper.Map<UserUpdateDto>(user);
				return userToReturn;
			}

			return null;
		}
		catch (Exception e)
		{
			throw new Exception($"Erro ao tentar criar Usuário! Erro: {e.Message}");
		}
	}

	public async Task<UserUpdateDto?> GetUserByUserNameAsync(string username)
	{
		try
		{
			var user = await _userPersistence.GetUserByNameAsync(username);
			return _mapper.Map<UserUpdateDto>(user) ?? null;
		}
		catch (Exception e)
		{
			throw new Exception($"Erro ao tentar localizar usuário por nome! Erro: {e.Message}");
		}
	}

	public async Task<UserUpdateDto?> UpdateAccount(UserUpdateDto userUpdateDto)
	{
		try
		{
			var user = await _userPersistence.GetUserByNameAsync(userUpdateDto.UserName!);
			if (user is null) return null;

			userUpdateDto.Id = user.Id;
			_mapper.Map(userUpdateDto, user);

			if (userUpdateDto.Password != null)
			{
				var token = await _userManager.GeneratePasswordResetTokenAsync(user);
				await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password!);
			}

			_userPersistence.Update(user);

			if (await _userPersistence.SaveChangesAsync())
			{
				var userToReturn = await _userPersistence.GetUserByNameAsync(userUpdateDto.UserName!);
				return _mapper.Map<UserUpdateDto>(userToReturn);
			}

			return null;
		}
		catch (Exception e)
		{
			throw new Exception($"Erro ao tentar atualizar usuário! Erro: {e.Message}");
		}
	}

	public async Task<bool> UserExists(string username)
	{
		try
		{
			//return await _userManager.Users.AnyAsync(us => 
			//	string.Equals(us.UserName!, username, StringComparison.CurrentCultureIgnoreCase));

			return await _userManager.Users.AnyAsync(us => us.UserName!.ToLower().Equals(username.ToLower()));
		}
		catch (Exception e)
		{
			throw new Exception($"O usuário consultado não pôde ser encontrado! Erro: {e.Message}");
		}
	}
}
