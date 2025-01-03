using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Api.Utils;
using ProEventos.Application.Dtos;
using System.Security.Claims;

namespace ProEventos.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
	private readonly IAccountService _accountService;
	private readonly ITokenService _tokenService;
	private readonly IUtil _util;
	private readonly string _folder = "Images";

	public AccountController(IAccountService accountService, ITokenService tokenService, IUtil util)
	{
		_accountService = accountService;
		_tokenService = tokenService;
		_util = util;
	}

	[HttpGet("GetUser")]
	// [AllowAnonymous] // Pq a classe possui o [Authorize], Esse decorator permite q o endpoint seja público
	public async Task<IActionResult> GetUser()
	{
		try
		{
			var userName = User.GetUserName();
			var user = await _accountService.GetUserByUserNameAsync(userName!);
			return Ok(user);
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError,
				$"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
		}
	}

	[HttpPost("Register")]
	[AllowAnonymous]
	public async Task<IActionResult> Register(UserDto userDto)
	{
		try
		{
			if (await _accountService.UserExists(userDto.UserName!))
				return BadRequest("Usuário já existe.");

			var user = await _accountService.CreateAccountAsync(userDto);
			return user is null
				? BadRequest("Erro ao tentar registrar Usuário.")
				: Ok(new
				{
					username = user.UserName,
					primeiroNome = user.PrimeiroNome,
					token = _tokenService.CreateToken(user).Result
				});
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError,
				$"Erro ao tentar registrar Usuário. Erro: {ex.Message}");
		}
	}

	[HttpPost("Login")]
	public async Task<IActionResult> Login(UserLoginDto userLoginDto)
	{
		try
		{
			var user = await _accountService.GetUserByUserNameAsync(userLoginDto.UserName!);
			if (user == null) return Unauthorized("Usuário não encontrado.");

			var result = await _accountService.CheckUserPasswordAsync(user, userLoginDto.Password!);

			return !result.Succeeded
				? Unauthorized("Usuário ou senha incorretos.")
				: Ok(new
				{
					username = user.UserName,
					primeiroNome = user.PrimeiroNome,
					token = _tokenService.CreateToken(user).Result
				});
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError,
				$"Erro ao tentar logar. Erro: {ex.Message}");
		}
	}

	[HttpPut("UpdateUser")]
	public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
	{
		try
		{
			var userName = User.GetUserName();
			var user = await _accountService.GetUserByUserNameAsync(userName!);

			if (user == null) return Unauthorized("Usuário inválido.");

			var updatedUser = await _accountService.UpdateAccount(userUpdateDto);

			return updatedUser is null
				? NoContent()
				: Ok(new
				{
					username = updatedUser.UserName,
					primeiroNome = updatedUser.PrimeiroNome,
					token = _tokenService.CreateToken(updatedUser).Result
				});
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError,
				$"Erro ao tentar atualizar Usuário. Erro: {ex.Message}");
		}
	}

	[HttpPost("upload-image")]
	public async Task<IActionResult> UploadImage()
	{
		try
		{
			var user = await _accountService.GetUserByUserNameAsync(User.GetUserName()!);
			if (user == null) return NoContent();

			var file = Request.Form.Files[0];
			if (file.Length > 0)
			{
				_util.DeleteImage(_folder, user.ImagemURL!);
				user.ImagemURL = await _util.SaveImage(file, _folder);
			}
			var userRetorno = await _accountService.UpdateAccount(user);

			return Ok(userRetorno);
		}
		catch (Exception ex)
		{
			return StatusCode(StatusCodes.Status500InternalServerError,
				$"Erro ao tentar realizar upload de Foto do Usuário. Erro: {ex.Message}");
		}
	}
}
