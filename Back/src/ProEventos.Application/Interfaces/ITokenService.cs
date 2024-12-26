using ProEventos.Application.Dtos;

namespace ProEventos.Application.Interfaces;

public interface ITokenService
{
	Task<string> CreateToken(UserUpdateDto userUpdateDto);
}
