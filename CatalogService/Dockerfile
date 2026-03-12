FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CatalogService/CatalogService.csproj", "CatalogService/"]
COPY ["Play.Common/Play.Common.csproj", "Play.Common/"]

RUN dotnet restore "CatalogService/CatalogService.csproj"

COPY . .

WORKDIR /src/CatalogService
RUN dotnet publish "CatalogService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CatalogService.dll"]