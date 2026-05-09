using System.Text;

namespace Travel_Explorer.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetAsync(object id);
        Task Delete(T Id);
        Task Update(object id);
        Task<T> GenericEntitiesWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListSpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}
