FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CrossCutting/HealthMonitoring/HealthMonitoring.csproj", "src/CrossCutting/HealthMonitoring/"]
RUN dotnet restore "src/CrossCutting/HealthMonitoring/HealthMonitoring.csproj"
COPY . .
WORKDIR "/src/src/CrossCutting/HealthMonitoring"
RUN dotnet build "HealthMonitoring.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HealthMonitoring.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthMonitoring.dll"]
