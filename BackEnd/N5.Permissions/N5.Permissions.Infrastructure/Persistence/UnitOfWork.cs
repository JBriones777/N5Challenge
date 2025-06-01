using Microsoft.Extensions.DependencyInjection;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Infrastructure.Persistence;

public class UnitOfWork(IServiceProvider serviceProvider, PermissionsDbContext permissionsDbContext) : IUnitOfWork
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly PermissionsDbContext _permissionsDbContext = permissionsDbContext;

    public virtual IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity => _serviceProvider.GetRequiredService<IBaseRepository<TEntity>>();
    public virtual async Task SaveChangesAsync() => await _permissionsDbContext.SaveChangesAsync();
}
