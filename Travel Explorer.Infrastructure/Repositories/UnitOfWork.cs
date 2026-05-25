using Travel_Explorer.Infrastructure.Data;

namespace Travel_Explorer.Infrastructure.Repositories
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {

        private readonly ApplicationDbContext _context = context;
        private readonly Dictionary<string, object> _repository = [];

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;

            if (!_repository.ContainsKey(type))
            {
                var repositoryInstance = new GenericRepository<TEntity>(_context);
                _repository.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repository[type];
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose() => _context.Dispose();
    }
}
