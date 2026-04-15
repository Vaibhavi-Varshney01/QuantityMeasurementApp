# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["QuantityMeasurementWebAPI/QuantityMeasurementWebAPI.csproj", "QuantityMeasurementWebAPI/"]
COPY ["QuantityMeasurementBusinessLayer/QuantityMeasurementBusinessLayer.csproj", "QuantityMeasurementBusinessLayer/"]
COPY ["QuantityMeasurementModel/QuantityMeasurementModel.csproj", "QuantityMeasurementModel/"]
COPY ["QuantityMeasurementRepository/QuantityMeasurementRepository.csproj", "QuantityMeasurementRepository/"]

RUN dotnet restore "QuantityMeasurementWebAPI/QuantityMeasurementWebAPI.csproj"

# Copy the rest of the code
COPY . .

# Build and publish
WORKDIR "/src/QuantityMeasurementWebAPI"
RUN dotnet publish "QuantityMeasurementWebAPI.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (Render sets PORT env var, but we can default to 8080)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "QuantityMeasurementWebAPI.dll"]
