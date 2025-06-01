using N5.Permissions.Application.UsesCases.PermissionOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.UsesCases.PermissionOperations.Queries.GetList;

public class GetListPermissions(IUnitOfWork unitOfWork) : IGetListPermissions
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<List<GetListPermissionDto>> DoIt()
    {
        var result = await _unitOfWork.GetRepository<Permiso>().GetListAsync(x => new GetListPermissionDto
        {
            Id = x.Id,
            NombreEmpleado = x.NombreEmpleado,
            ApellidoEmpleado = x.ApellidoEmpleado,
            TipoPermiso = x.TipoPermiso.Descripcion,
            FechaPermiso = x.FechaPermiso
        }, false);

        return result;
    }
}
