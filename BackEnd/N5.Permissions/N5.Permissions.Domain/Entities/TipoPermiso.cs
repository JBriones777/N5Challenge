namespace N5.Permissions.Domain.Entities;

public partial class TipoPermiso : BaseEntity
{
    public string Descripcion { get; set; }

    public virtual ICollection<Permiso> Permissions { get; set; } = new List<Permiso>();
}