using Microsoft.EntityFrameworkCore;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace N5.Permissions.Infrastructure.Repositories;

public class BaseRepository<TEntity>(PermissionsDbContext context, IReadOnlyDbContext readOnlyDbContext) : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DbSet<TEntity> _entity = context.Set<TEntity>();
    protected readonly DbSet<TEntity> _readOnlyEntity = readOnlyDbContext.Set<TEntity>();

    public virtual async Task AddAsync(TEntity entity) => await _entity.AddAsync(entity);
    public virtual void Update(TEntity entity) => _entity.Update(entity);
    public virtual async Task<TEntity?> GetByIdAsync(int id) => await _readOnlyEntity.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    public virtual async Task<bool> ExistsAsync(int id) => await _readOnlyEntity.AsNoTracking().AnyAsync(x => x.Id == id);
    public virtual async Task<List<TEntity>> GetListAsync(bool tracking = true)
    {
        var query = _readOnlyEntity.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }
    public virtual async Task<List<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, bool tracking = true)
    {
        var query = _readOnlyEntity.AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();

        return await query.Select(selector).ToListAsync();
    }
}
