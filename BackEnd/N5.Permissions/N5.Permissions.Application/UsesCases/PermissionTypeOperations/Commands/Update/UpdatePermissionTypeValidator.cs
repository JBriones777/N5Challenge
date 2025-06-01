using FluentValidation;
using N5.Permissions.Application.UsesCases.PermissionTypeOperations.Dtos;

namespace N5.Permissions.Application.UsesCases.PermissionTypeOperations.Commands.Update;

public class UpdatePermissionTypeValidator : AbstractValidator<PermissionTypeDto>
{
    public UpdatePermissionTypeValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id debe ser distinto de 0.");

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(255).WithMessage("La descripción no puede exceder los 255 caracteres.");
    }
}