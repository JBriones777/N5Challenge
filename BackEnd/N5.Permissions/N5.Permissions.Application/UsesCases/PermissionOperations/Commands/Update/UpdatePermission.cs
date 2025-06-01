using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using N5.Permissions.Application.UsesCases.PermissionOperations.Contracts;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Application.UsesCases.PermissionOperations.Commands.Update;

public class UpdatePermission(IUnitOfWork unitOfWork,
    [FromKeyedServices("Update")] IValidator<PermissionDto> validator
    ) : IUpdatePermission
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<PermissionDto> _validator = validator;

    public async Task<PermissionDto> DoIt(PermissionDto permission)
    {
        var isValid = await _validator.ValidateAsync(permission);
        if (!isValid.IsValid)
            throw new ValidationException(isValid.Errors);

        var existEntity = await _unitOfWork.GetRepository<Permiso>().ExistsAsync(permission.Id);
        if (!existEntity)
            throw new KeyNotFoundException($"Permiso: {permission.Id} no existe.");

        var entity = permission.ToEntity();
        _unitOfWork.GetRepository<Permiso>().Update(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.ToDto();
    }
}
