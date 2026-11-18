# syntax=docker/dockerfile:1
#
# StockScribe API · multi-stage build. Build context: apps/api
#
#   target `api`      → ASP.NET Core runtime image that serves the API on :8080
#   target `migrator` → runs the EF Core migration bundle once, then exits
#
#   docker build --target api      -t stockscribe-api      apps/api
#   docker build --target migrator -t stockscribe-migrator apps/api

ARG DOTNET_VERSION=10.0

# ---------- build: restore + publish ----------
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

# Restore first so the package layer stays cached until the csproj changes.
COPY StockScribe.Api/StockScribe.Api.csproj StockScribe.Api/
RUN dotnet restore StockScribe.Api/StockScribe.Api.csproj

COPY StockScribe.Api/ StockScribe.Api/
RUN dotnet publish StockScribe.Api/StockScribe.Api.csproj \
      --configuration Release --no-restore \
      --output /app/publish /p:UseAppHost=false

# ---------- bundle: EF Core migration bundle ----------
FROM build AS bundle
ARG EF_VERSION=10.0.15
RUN dotnet tool install --global dotnet-ef --version ${EF_VERSION}
ENV PATH="${PATH}:/root/.dotnet/tools"
# The design-time DbContext needs *a* connection string to build; the real one is passed at run time.
ENV ConnectionStrings__Postgres="Host=build-only;Database=build-only"
RUN dotnet ef migrations bundle \
      --project StockScribe.Api/StockScribe.Api.csproj \
      --configuration Release \
      --output /out/efbundle

# ---------- migrator ----------
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS migrator
WORKDIR /app
COPY --from=bundle /out/efbundle ./efbundle
USER $APP_UID
# Reads the same variable as the API so compose only defines it once.
ENTRYPOINT ["sh", "-c", "exec ./efbundle --connection \"$ConnectionStrings__Postgres\""]

# ---------- api (default target) ----------
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS api
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "StockScribe.Api.dll"]
