using System.Text;
using Travel_Explorer.Infrastructure.Data;

namespace Travel_Explorer.Infrastructure.Repositories
{
    public class UnitOfWork :IUnitOfWork
    {

        private readonly ApplicationDbContext _context;
        private readonly Dictionary<string, object> _repository;



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repository = new Dictionary<string, object>();
        }

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
