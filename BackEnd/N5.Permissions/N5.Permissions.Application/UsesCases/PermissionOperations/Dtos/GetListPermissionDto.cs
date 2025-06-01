namespace N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;

public class GetListPermissionDto 
{
    public int Id { get; set; }
    public string NombreEmpleado { get; set; }

    public string ApellidoEmpleado { get; set; }

    public string TipoPermiso { get; set; }

    public DateTime FechaPermiso { get; set; }
}
