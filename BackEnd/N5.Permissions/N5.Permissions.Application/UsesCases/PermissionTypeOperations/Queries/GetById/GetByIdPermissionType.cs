using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.UsesCases.PermissionTypeOperations.Queries.GetById;

public class GetByIdPermissionType(IUnitOfWork unitOfWork) : IGetByIdPermissionType
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PermissionTypeDto> DoIt(int id)
    {
        var result = await _unitOfWork.GetRepository<TipoPermiso>().GetByIdAsync(id);
        return result == null ? throw new KeyNotFoundException($"Tipo de permiso: {id} no existe.") : result.ToDto();
    }
}
