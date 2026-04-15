FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY shared-models/ shared-models/
COPY auth-service/ auth-service/
RUN dotnet restore auth-service/auth-service.csproj
RUN dotnet publish auth-service/auth-service.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "auth-service.dll"]
