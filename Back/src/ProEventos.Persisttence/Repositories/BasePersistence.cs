using ProEventos.Persisttence.Context;
using ProEventos.Persisttence.Interfaces;

namespace ProEventos.Persisttence.Repositories;

public class BasePersistence : IBasePersistence
{
    private readonly ProEventosContext _context;

    public BasePersistence(ProEventosContext context)
        => _context = context;

    public void Add<T>(T entity) where T : class
        => _context.AddAsync(entity);

    public void Update<T>(T entity) where T : class
        => _context.Update(entity);

    public void Delete<T>(T entity) where T : class
        => _context.Remove(entity);

    public void DeleteRange<T>(T[] entitiesArray) where T : class
        => _context.RemoveRange(entitiesArray);

    public async Task<bool> SaveChangesAsync()
        => await _context.SaveChangesAsync() > 0;

}
