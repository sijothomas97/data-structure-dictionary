# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore first (layer-cached unless csproj files change)
COPY StudentManager.Domain/StudentManager.Domain.csproj StudentManager.Domain/
COPY StudentManager.Infrastructure/StudentManager.Infrastructure.csproj StudentManager.Infrastructure/
COPY StudentManager.Api/StudentManager.Api.csproj StudentManager.Api/
RUN dotnet restore StudentManager.Api/StudentManager.Api.csproj

# Build + publish
COPY StudentManager.Domain/ StudentManager.Domain/
COPY StudentManager.Infrastructure/ StudentManager.Infrastructure/
COPY StudentManager.Api/ StudentManager.Api/
RUN dotnet publish StudentManager.Api/StudentManager.Api.csproj -c Release -o /app/publish --no-restore

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Writable directory for the default SQLite database (mount a volume here).
RUN mkdir -p /data && chown app:app /data
ENV ConnectionStrings__Default="Data Source=/data/students.db"

USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "StudentManager.Api.dll"]
