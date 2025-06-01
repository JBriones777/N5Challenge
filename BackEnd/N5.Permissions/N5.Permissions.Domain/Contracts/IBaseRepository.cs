using N5.Permissions.Domain.Entities;
using System.Linq.Expressions;

namespace N5.Permissions.Domain.Contracts;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);
    Task<bool> ExistsAsync(int id);
    Task<TEntity?> GetByIdAsync(int id);
    Task<List<TEntity>> GetListAsync(bool tracking = true);
    void Update(TEntity entity);
    Task<List<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, bool tracking = true);
}