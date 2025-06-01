using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Domain.Contracts;

public interface IUnitOfWork
{
    IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
    Task SaveChangesAsync();
}