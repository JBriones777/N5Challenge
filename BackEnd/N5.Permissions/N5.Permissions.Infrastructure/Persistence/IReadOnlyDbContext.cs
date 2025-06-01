using Microsoft.EntityFrameworkCore;

namespace N5.Permissions.Infrastructure.Persistence;

public interface IReadOnlyDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
