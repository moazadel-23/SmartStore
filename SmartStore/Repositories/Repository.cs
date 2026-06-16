using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SmartStore.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        ILogger<Repository<T>> _logger;
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _db;

        public Repository(ILogger<Repository<T>> logger, ApplicationDbContext context)
        {
            _logger = logger; 
            _context = context;
            _db = context.Set<T>();
        }
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await _db.AddAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding entity of type {typeof(T).Name}");
            }
        }
        public void Update(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _db.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding entity of type {typeof(T).Name}");
            }
        }
        public void Delete(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _db.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding entity of type {typeof(T).Name}");
            }
        }
        public async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>> []?include = null,
            bool tracked = true,CancellationToken cancellationToken = default)
        {
            var entities = _db.AsQueryable();

            if(expression is not null)
                entities = entities.Where(expression);
            if (include is not null)
            {
                foreach (var item in include)
                {
                    entities = entities.Include(item);
                }
            }
            if(!tracked)
                entities = entities.AsNoTracking();

            return await entities.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? include = null,
            bool tracked = true, CancellationToken cancellationToken = default)
        {
            return (await GetAsync(expression, include, tracked, cancellationToken)).FirstOrDefault();
        }

        public async Task Commit(CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
