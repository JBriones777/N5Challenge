# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY N5.Permissions.sln ./
COPY N5.Permissions.Domain ./N5.Permissions.Domain
COPY N5.Permissions.Application ./N5.Permissions.Application
COPY N5.Permissions.Infrastructure ./N5.Permissions.Infrastructure
COPY N5.Permissions.API ./N5.Permissions.API

RUN dotnet restore ./N5.Permissions.API/N5.Permissions.API.csproj
RUN dotnet publish ./N5.Permissions.API/N5.Permissions.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "N5.Permissions.API.dll"]