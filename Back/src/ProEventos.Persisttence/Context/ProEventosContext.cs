using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Entities;
using ProEventos.Domain.Identity;

namespace ProEventos.Persisttence.Context;

public class ProEventosContext : IdentityDbContext<User, Role, int, 
                                    IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, 
                                    IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Lote> Lotes { get; set; }
    public DbSet<Palestrante> Palestrantes { get; set; }
    public DbSet<PalestranteEvento> PalestranteEventos { get; set; }
    public DbSet<RedeSocial> RedeSocials { get; set; }

    public ProEventosContext(DbContextOptions<ProEventosContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PalestranteEvento>()
            .HasKey(PE => new { PE.EventoId, PE.PalestranteId });

        builder.Entity<Evento>()
            .HasMany(e => e.RedesSociais)
            .WithOne(rs => rs.Evento)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Palestrante>()
            .HasMany(e => e.RedesSociais)
            .WithOne(rs => rs.Palestrante)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
            
            userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

			userRole.HasOne(ur => ur.User)
					.WithMany(r => r.UserRoles)
					.HasForeignKey(ur => ur.UserId)
					.IsRequired();
		});
    }
}
