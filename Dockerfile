FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/apsent: 5.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "InventoryService.dll"]