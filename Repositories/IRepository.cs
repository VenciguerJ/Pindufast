namespace PinduFast.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(int id);
    Task Add(T entity);
    Task Update(T entity);
    Task<IEnumerable<T>> BuscaComplete(string? searchString, int? IsActive);
    Task Delete(int id);
}