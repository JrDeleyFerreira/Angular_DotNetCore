namespace ProEventos.Persisttence.Interfaces;

public interface IBasePersistence
{
    //GERAL
    void Add<T>(T entity) where T : class;
    void Update<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    void DeleteRange<T>(T[] entitiesArray) where T : class;
    Task<bool> SaveChangesAsync();
}
