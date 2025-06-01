namespace N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;

public class PermissionDto
{
    public int Id { get; set; }
    public string NombreEmpleado { get; set; }

    public string ApellidoEmpleado { get; set; }

    public int TipoPermisoId { get; set; }
}
