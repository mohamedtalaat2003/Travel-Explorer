using Microsoft.EntityFrameworkCore;
using System.Text;
using Travel_Explorer.Infrastructure.Data;

namespace Travel_Explorer.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public async Task Delete(T Id)
        {
            var entity = await _context.Set<T>().FindAsync(Id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }
        public async Task<T> GenericEntitiesWithSpec(ISpecification<T> spec) => await ApplySpecification(spec).FirstOrDefaultAsync();


        public async Task<IReadOnlyList<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
        public async Task<T> GetAsync(object id) => await _context.Set<T>().FindAsync(id);
        public async Task Update(object id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Update(entity);
            }
        }

        public async Task<IReadOnlyList<T>> ListSpecAsync(ISpecification<T> spec) => await ApplySpecification(spec).ToListAsync();

        /// <inheritdoc/>
        public async Task<int> CountAsync(ISpecification<T> spec)
            => await ApplySpecification(spec).CountAsync();

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
            => SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);

    }
}
