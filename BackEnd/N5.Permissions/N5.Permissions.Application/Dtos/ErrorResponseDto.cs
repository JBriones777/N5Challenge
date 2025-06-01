namespace N5.Permissions.Application.Dtos;

public class ErrorResponseDto
{
    public string Message { get; set; }
    public IEnumerable<ErrorModel> Details { get; set; }
}
public class ErrorModel
{
    public string Code { get; set; }
    public string PropertyName { get; set; }
    public string Message { get; set; }
}