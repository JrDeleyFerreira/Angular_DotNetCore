using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProEventos.Application.Services;

public class TokenService : ITokenService
{
	private readonly IConfiguration _config;
	private readonly UserManager<User> _userManager;
	private readonly IMapper _mapper;
	private readonly SymmetricSecurityKey _key;

	public TokenService(IConfiguration config, UserManager<User> userManager, IMapper mapper)
	{
		_config = config;
		_userManager = userManager;
		_mapper = mapper;
		_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
	}

	public async Task<string> CreateToken(UserUpdateDto userUpdateDto)
	{
		var user = _mapper.Map<User>(userUpdateDto);

		var claims = new List<Claim>
		{
			new (ClaimTypes.NameIdentifier, user.Id.ToString()),
			new (ClaimTypes.Name, user.UserName!)
		};

		var roles = await _userManager.GetRolesAsync(user);

		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

		var tokenDescription = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.Now.AddDays(1),
			SigningCredentials = creds
		};

		var tokenHandler = new JwtSecurityTokenHandler();

		var token = tokenHandler.CreateToken(tokenDescription);

		return tokenHandler.WriteToken(token);
	}
}
