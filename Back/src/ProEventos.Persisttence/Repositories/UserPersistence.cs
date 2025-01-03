using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Persisttence.Context;
using ProEventos.Persisttence.Interfaces;

namespace ProEventos.Persisttence.Repositories;

public class UserPersistence : BasePersistence, IUserPersistence
{
	private readonly ProEventosContext _context;
	public UserPersistence(ProEventosContext context) : base(context) => _context = context;

	public async Task<IEnumerable<User>?> GetUsersAsync() => await _context.Users.ToListAsync();

	public async Task<User?> GetUserByIdAsync(int id)
		=> await _context.Users.SingleOrDefaultAsync(user => user.Id == id);

	public async Task<User?> GetUserByNameAsync(string name)
		=> await _context.Users.SingleOrDefaultAsync(
			user => user.UserName!.ToLower().Equals(name.ToLower()));
}
