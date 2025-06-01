namespace N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;

public class GetPermissionByIdDto
{
    public int Id { get; set; }
    public string NombreEmpleado { get; set; }

    public string ApellidoEmpleado { get; set; }

    public int TipoPermisoId { get; set; }
    public DateTime FechaPermiso { get; set; }
}
