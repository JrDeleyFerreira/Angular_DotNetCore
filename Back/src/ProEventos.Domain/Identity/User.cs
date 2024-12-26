using Microsoft.AspNetCore.Identity;
using ProEventos.Domain.Enums;

namespace ProEventos.Domain.Identity;

public class User : IdentityUser<int>
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public Titulo Education { get; set; }
	public string? Description { get; set; }
	public Funcao Function { get; set; }
	public string? ImageUrl { get; set; }
	public IEnumerable<UserRole>? UserRoles { get; set; }
}
