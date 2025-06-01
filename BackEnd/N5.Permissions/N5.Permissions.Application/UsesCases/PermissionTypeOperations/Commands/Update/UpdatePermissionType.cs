using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.UsesCases.PermissionTypeOperations.Commands.Update;

public class UpdatePermissionType(IUnitOfWork unitOfWork,
    [FromKeyedServices("Update")] IValidator<PermissionTypeDto> validator
    ) : IUpdatePermissionType
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<PermissionTypeDto> _validator = validator;

    public async Task<PermissionTypeDto> DoIt(PermissionTypeDto permissionType)
    {
        var isValid = await _validator.ValidateAsync(permissionType);
        if (!isValid.IsValid)
            throw new ValidationException(isValid.Errors);
        var existEntity = await _unitOfWork.GetRepository<TipoPermiso>().ExistsAsync(permissionType.Id);
        if (!existEntity)
            throw new KeyNotFoundException($"Tipo de permiso: {permissionType.Id} no existe.");

        var entity = permissionType.ToEntity();
        _unitOfWork.GetRepository<TipoPermiso>().Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.ToDto();
    }
}
