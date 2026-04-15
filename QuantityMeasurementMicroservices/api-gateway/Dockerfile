FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY shared-models/ shared-models/
COPY api-gateway/ api-gateway/
RUN dotnet restore api-gateway/api-gateway.csproj
RUN dotnet publish api-gateway/api-gateway.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "api-gateway.dll"]
