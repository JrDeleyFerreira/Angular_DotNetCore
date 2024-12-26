using ProEventos.Domain.Identity;

namespace ProEventos.Persisttence.Interfaces;

public interface IUserPersistence : IBasePersistence
{
	Task<IEnumerable<User>?> GetUsersAsync();
	Task<User?> GetUserByIdAsync(int id);
	Task<User?> GetUserByNameAsync(string name);
}
