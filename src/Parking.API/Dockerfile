FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Parking.API/Parking.API.csproj", "src/Parking.API/"]
COPY ["src/Parking.IoC/Parking.IoC.csproj", "src/Parking.IoC/"]
COPY ["src/Parking.Data/Parking.Data.csproj", "src/Parking.Data/"]
COPY ["src/Parking.Domain/Parking.Domain.csproj", "src/Parking.Domain/"]
COPY ["src/Parking.Service/Parking.Service.csproj", "src/Parking.Service/"]

RUN dotnet restore "src/Parking.API/Parking.API.csproj"

COPY . .
WORKDIR "/src/src/Parking.API"
RUN dotnet publish "Parking.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Parking.API.dll"]