using FluentValidation;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;

namespace N5.Permissions.Application.UsesCases.PermissionOperations.Commands.Add;

public class AddPermissionValidator : AbstractValidator<PermissionDto>
{
    public AddPermissionValidator()
    {
        RuleFor(x=>x.Id)
            .Equal(0).WithMessage("El Id no puede ser distinto de 0.");
        RuleFor(x => x.NombreEmpleado)
            .NotEmpty().WithMessage("El nombre del empleado es obligatorio.")
            .MaximumLength(255).WithMessage("El nombre del empleado no puede exceder los 255 caracteres.");
        RuleFor(x => x.ApellidoEmpleado)
            .NotEmpty().WithMessage("El apellido del empleado es obligatorio.")
            .MaximumLength(255).WithMessage("El apellido del empleado no puede exceder los 255 caracteres.");
        RuleFor(x => x.TipoPermisoId)
            .GreaterThan(0).WithMessage("El tipo de permiso debe ser un valor distinto de 0.");
    }
}
