using Travel_Explorer.Infrastructure.Data;

namespace Travel_Explorer.Infrastructure.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public async Task Delete(object id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                if (entity is BaseEntity softDeleteEntity)
                {
                    softDeleteEntity.IsDeleted = true;
                    softDeleteEntity.UpdatedAt = DateTime.UtcNow; 

                    _context.Set<T>().Update(entity);
                }
                else
                {
                    _context.Set<T>().Remove(entity);
                }
            }
        }
        public async Task<T> GenericEntitiesWithSpec(ISpecification<T> spec) => await ApplySpecification(spec).FirstOrDefaultAsync();


        public async Task<IReadOnlyList<T>> GetAllAsync() => await _context.Set<T>().AsNoTracking().ToListAsync();
        public async Task<T> GetAsync(object id) => await _context.Set<T>().FindAsync(id);
        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IReadOnlyList<T>> ListSpecAsync(ISpecification<T> spec) => await ApplySpecification(spec).AsNoTracking().ToListAsync();

        
        public async Task<int> CountAsync(ISpecification<T> spec)
            => await ApplySpecification(spec, true).CountAsync();

        private IQueryable<T> ApplySpecification(ISpecification<T> spec, bool ignorePaging = false)
            => SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec, ignorePaging);

    }
}
