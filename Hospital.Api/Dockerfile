# Use the official .NET 8 SDK image for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files from solution root context
COPY HospitalSystem.sln ./
COPY Hospital.Api/Hospital.Api.csproj Hospital.Api/
COPY Hospital.Core/Hospital.Core.csproj Hospital.Core/
COPY Hospital.Infrastructure/Hospital.Infrastructure.csproj Hospital.Infrastructure/

# Restore dependencies
RUN dotnet restore Hospital.Api/Hospital.Api.csproj

# Copy full source from solution root context
COPY Hospital.Api/ ./Hospital.Api/
COPY Hospital.Core/ ./Hospital.Core/
COPY Hospital.Infrastructure/ ./Hospital.Infrastructure/

# Build and publish the API
RUN dotnet publish Hospital.Api/Hospital.Api.csproj -c Release -o /app/publish

# Use the ASP.NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Expose port 8080 (matches docker-compose.yml)
EXPOSE 8080

# Run the API
ENTRYPOINT ["dotnet", "Hospital.Api.dll"]
