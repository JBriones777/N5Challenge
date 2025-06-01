using FluentValidation;
using FluentValidation.Results;
using Moq;
using N5.Permissions.Application.UsesCases.PermissionOperations.Commands.Update;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.UnitTest;

public class UpdatePermissionTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBaseRepository<Permiso>> _permisoRepoMock = new();
    private readonly Mock<IValidator<PermissionDto>> _validatorMock = new();

    private UpdatePermission CreateSut()
    {
        _unitOfWorkMock.Setup(u => u.GetRepository<Permiso>()).Returns(_permisoRepoMock.Object);
        return new UpdatePermission(_unitOfWorkMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Should_Throw_ValidationException_When_Model_Is_Invalid()
    {
        // Arrange
        var dto = new PermissionDto();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("NombreEmpleado", "Required") });
        _validatorMock.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        var sut = CreateSut();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => sut.DoIt(dto));
    }

    [Fact]
    public async Task Should_Throw_KeyNotFoundException_When_Permission_Not_Exists()
    {
        // Arrange
        var dto = new PermissionDto { Id = 99 };
        _validatorMock.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _permisoRepoMock.Setup(r => r.ExistsAsync(dto.Id)).ReturnsAsync(false);

        var sut = CreateSut();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => sut.DoIt(dto));
    }

    [Fact]
    public async Task Should_Update_Permission_And_Return_Dto_When_Valid()
    {
        // Arrange
        var dto = new PermissionDto { Id = 1, NombreEmpleado = "John", ApellidoEmpleado = "Doe", TipoPermisoId = 1 };
        _validatorMock.Setup(v => v.ValidateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _permisoRepoMock.Setup(r => r.ExistsAsync(dto.Id)).ReturnsAsync(true);

        var permisoEntity = new Permiso { Id = 1, NombreEmpleado = "John", ApellidoEmpleado = "Doe", TipoPermisoId = 1 };

        _permisoRepoMock.Setup(r => r.Update(It.IsAny<Permiso>()));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        var sut = CreateSut();

        // Act
        var result = await sut.DoIt(dto);

        // Assert
        _permisoRepoMock.Verify(r => r.Update(It.IsAny<Permiso>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        Assert.Equal(dto.NombreEmpleado, result.NombreEmpleado);
        Assert.Equal(dto.ApellidoEmpleado, result.ApellidoEmpleado);
        Assert.Equal(dto.TipoPermisoId, result.TipoPermisoId);
    }
}
