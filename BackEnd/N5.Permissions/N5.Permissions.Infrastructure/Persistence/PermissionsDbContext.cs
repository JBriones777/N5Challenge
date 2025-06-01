using Microsoft.EntityFrameworkCore;
using N5.Permissions.Domain.Entities;
using System.Reflection;

namespace N5.Permissions.Infrastructure.Persistence;

public partial class PermissionsDbContext(DbContextOptions<PermissionsDbContext> options) : DbContext(options), IReadOnlyDbContext
{
    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<TipoPermiso> TipoPermisos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

}