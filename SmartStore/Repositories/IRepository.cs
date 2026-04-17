using System.Linq.Expressions;

namespace SmartStore.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity, CancellationToken cancellationToken = default);
        void Delete(T entity, CancellationToken cancellationToken = default);
        Task Commit(CancellationToken cancellationToken = default);


        Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? include = null,
            bool tracked = true,
            CancellationToken cancellationToken = default);

       Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? include = null,
            bool tracked = true,
            CancellationToken cancellationToken = default);
      
    }
}
