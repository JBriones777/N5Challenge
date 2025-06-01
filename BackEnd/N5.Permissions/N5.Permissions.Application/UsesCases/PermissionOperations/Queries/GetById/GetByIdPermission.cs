using N5.Permissions.Application.UsesCases.PermissionOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.UsesCases.PermissionOperations.Queries.GetById;

public class GetByIdPermission(IUnitOfWork unitOfWork) : IGetByIdPermission
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetPermissionByIdDto> DoIt(int id)
    {
        var result = await _unitOfWork.GetRepository<Permiso>().GetByIdAsync(id);
        return result == null ? throw new KeyNotFoundException($"Permiso: {id} no existe.") : result.ToSingleDto();
    }
}
