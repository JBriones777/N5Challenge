using N5.Permissions.API.EnpointsExtensions;
using N5.Permissions.Infrastructure.Extensions;
using N5.Permissions.Application.Extensions;
using N5.Permissions.API.Middlewares;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowSpecificOrigins",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();

                      });
});
var app = builder.Build();

app.UseMiddleware<GlobalExceptionCatcher>();

app.UseCors("AllowSpecificOrigins");

app.MapOpenApi();

app.UseSwagger();

app.UseSwaggerUI();

app.MapPermission();

app.MapPermissionType();

app.CreateDatabase();

app.UseHttpsRedirection();

app.Run();

public partial class Program;