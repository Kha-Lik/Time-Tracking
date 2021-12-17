#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Identity/TimeTracking.Identity.WebApi/TimeTracking.Identity.WebApi.csproj", "src/Identity/TimeTracking.Identity.WebApi/"]
COPY ["src/Identity/TimeTracking.Identity.Dal.Impl/TimeTracking.Identity.Dal.Impl.csproj", "src/Identity/TimeTracking.Identity.Dal.Impl/"]
COPY ["src/TimeTracking.Common/TimeTracking.Common.csproj", "src/TimeTracking.Common/"]
COPY ["src/TimeTracking.Templates/TimeTracking.Templates.csproj", "src/TimeTracking.Templates/"]
COPY ["src/Identity/TimeTracking.Identity.Dal.Abstract/TimeTracking.Identity.Dal.Abstract.csproj", "src/Identity/TimeTracking.Identity.Dal.Abstract/"]
COPY ["src/Identity/TimeTracking.Identity.Entities/TimeTracking.Identity.Entities.csproj", "src/Identity/TimeTracking.Identity.Entities/"]
COPY ["src/TimeTracking.Contracts/TimeTracking.Contracts.csproj", "src/TimeTracking.Contracts/"]
COPY ["src/Identity/TimeTracking.Identity.BL.Impl/TimeTracking.Identity.BL.Impl.csproj", "src/Identity/TimeTracking.Identity.BL.Impl/"]
COPY ["src/Identity/TimeTracking.Identity.BL.Abstract/TimeTracking.Identity.BL.Abstract.csproj", "src/Identity/TimeTracking.Identity.BL.Abstract/"]
COPY ["src/Identity/TimeTracking.Identity.Models/TimeTracking.Identity.Models.csproj", "src/Identity/TimeTracking.Identity.Models/"]
RUN dotnet restore "src/Identity/TimeTracking.Identity.WebApi/TimeTracking.Identity.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Identity/TimeTracking.Identity.WebApi"
RUN dotnet build "TimeTracking.Identity.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeTracking.Identity.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeTracking.Identity.WebApi.dll"]