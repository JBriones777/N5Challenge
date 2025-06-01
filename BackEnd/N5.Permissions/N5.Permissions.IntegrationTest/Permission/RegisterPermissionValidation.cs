using N5.Permissions.Application.Dtos;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;
using N5.Permissions.IntegrationTest.Supports;
using System.Net;
using System.Net.Http.Json;

namespace N5.Permissions.IntegrationTest.Permission;

public class RegisterPermissionValidation : Given_When_Then_Test_Async, IClassFixture<IntegrationSupport>
{
    private readonly HttpClient _client;
    private HttpResponseMessage _result;
    private PermissionDto _permission;

    public RegisterPermissionValidation(IntegrationSupport fixture)
    {
        _client = fixture.Client;
        InitializeAsync().GetAwaiter().GetResult();
    }
    public static IEnumerable<object[]> InvalidLengthData()
    {
        yield return new object[]
        {
        new string('N', 1) + new string('a', 255), 
        "Apellido",
        1,
        "El nombre del empleado no puede exceder los 255 caracteres."
        };
        yield return new object[]
        {
        "Nombre",
        new string('A', 1) + new string('b', 255), 
        1,
        "El apellido del empleado no puede exceder los 255 caracteres."
        };
    }
    protected override async Task Given()
    {
        _permission = new PermissionDto
        {
            Id = 1,
            NombreEmpleado = "", 
            ApellidoEmpleado = "", 
            TipoPermisoId = 0 
        };
    }

    protected override async Task When()
    {
        _result = await _client.PostAsJsonAsync("/api/Permission", _permission);
    }

    [Fact]
    public async Task Then_Should_Return_Validation_Errors()
    {
        Assert.Equal(HttpStatusCode.BadRequest, _result.StatusCode);

        var errorResponse = await _result.Content.ReadFromJsonAsync<ErrorResponseDto>();
        Assert.NotNull(errorResponse);
        Assert.Equal("Error de validación.", errorResponse.Message);

        var details = errorResponse.Details.ToList();
        Assert.Contains(details, d => d.PropertyName == "Id" && d.Message == "El Id no puede ser distinto de 0.");
        Assert.Contains(details, d => d.PropertyName == "NombreEmpleado" && d.Message == "El nombre del empleado es obligatorio.");
        Assert.Contains(details, d => d.PropertyName == "ApellidoEmpleado" && d.Message == "El apellido del empleado es obligatorio.");
        Assert.Contains(details, d => d.PropertyName == "TipoPermisoId" && d.Message == "El tipo de permiso debe ser un valor distinto de 0.");
    }

    [Theory]
    [InlineData(null, "Apellido", 1, "El nombre del empleado es obligatorio.")]
    [InlineData("Nombre", null, 1, "El apellido del empleado es obligatorio.")]
    [InlineData("Nombre", "Apellido", 0, "El tipo de permiso debe ser un valor distinto de 0.")]
    [MemberData(nameof(InvalidLengthData))]
    public async Task Should_Return_Specific_Validation_Error(string nombre, string apellido, int tipoPermisoId, string expectedMessage)
    {
        var permission = new PermissionDto
        {
            Id = 0,
            NombreEmpleado = nombre,
            ApellidoEmpleado = apellido,
            TipoPermisoId = tipoPermisoId
        };

        var result = await _client.PostAsJsonAsync("/api/Permission", permission);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        var errorResponse = await result.Content.ReadFromJsonAsync<ErrorResponseDto>();
        Assert.NotNull(errorResponse);
        Assert.Equal("Error de validación.", errorResponse.Message);

        Assert.Contains(errorResponse.Details, d => d.Message == expectedMessage);
    }
}
