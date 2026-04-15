FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY shared-models/ shared-models/
COPY repository-service/ repository-service/
RUN dotnet restore repository-service/repository-service.csproj
RUN dotnet publish repository-service/repository-service.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "repository-service.dll"]
