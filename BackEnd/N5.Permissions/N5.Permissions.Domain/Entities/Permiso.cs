namespace N5.Permissions.Domain.Entities;

public partial class Permiso : BaseEntity
{
    public string NombreEmpleado { get; set; }

    public string ApellidoEmpleado { get; set; }

    public int TipoPermisoId { get; set; }

    public DateTime FechaPermiso { get; set; } = DateTime.UtcNow;

    public virtual TipoPermiso TipoPermiso { get; set; }
}