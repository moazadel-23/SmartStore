using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SmartStore.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _db;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _db = context.Set<T>();
        }
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _db.AddAsync(entity, cancellationToken);
        }
        public void Update(T entity, CancellationToken cancellationToken = default)
        {
             _db.Update(entity);
        }
        public void Delete(T entity, CancellationToken cancellationToken = default)
        {
            _db.Remove(entity);
        }
        public async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, object>> []include,
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

        public async Task<T?> GetOne(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, object>>[] include,
            bool tracked = true, CancellationToken cancellationToken = default)
        {
            return (await GetAsync(expression, include, tracked, cancellationToken)).FirstOrDefault();
        }

        public async Task Commit(CancellationToken cancellationToken)
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
