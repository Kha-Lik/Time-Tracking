#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["test/IntegrationTests/TimeTracking.IntegrationTests/TimeTracking.IntegrationTests.csproj", "test/IntegrationTests/TimeTracking.IntegrationTests/"]
COPY ["test/TimeTracking.Tests.Common/TimeTracking.Tests.Common.csproj", "test/TimeTracking.Tests.Common/"]
COPY ["src/TimeTracking/TimeTracking.Dal.Impl/TimeTracking.Dal.Impl.csproj", "src/TimeTracking/TimeTracking.Dal.Impl/"]
COPY ["src/TimeTracking/TimeTracking.Entities/TimeTracking.Entities.csproj", "src/TimeTracking/TimeTracking.Entities/"]
COPY ["src/TimeTracking.Common/TimeTracking.Common.csproj", "src/TimeTracking.Common/"]
COPY ["src/TimeTracking.Templates/TimeTracking.Templates.csproj", "src/TimeTracking.Templates/"]
COPY ["src/TimeTracking/TimeTracking.Dal.Abstract/TimeTracking.Dal.Abstract.csproj", "src/TimeTracking/TimeTracking.Dal.Abstract/"]
COPY ["src/TimeTracking/TimeTracking.Models/TimeTracking.Models.csproj", "src/TimeTracking/TimeTracking.Models/"]
COPY ["src/Identity/TimeTracking.Identity.Dal.Impl/TimeTracking.Identity.Dal.Impl.csproj", "src/Identity/TimeTracking.Identity.Dal.Impl/"]
COPY ["src/Identity/TimeTracking.Identity.Dal.Abstract/TimeTracking.Identity.Dal.Abstract.csproj", "src/Identity/TimeTracking.Identity.Dal.Abstract/"]
COPY ["src/Identity/TimeTracking.Identity.Entities/TimeTracking.Identity.Entities.csproj", "src/Identity/TimeTracking.Identity.Entities/"]
COPY ["src/TimeTracking/TimeTracking.WebApi/TimeTracking.WebApi.csproj", "src/TimeTracking/TimeTracking.WebApi/"]
COPY ["src/TimeTracking/TimeTracking.Bl.Impl/TimeTracking.Bl.Impl.csproj", "src/TimeTracking/TimeTracking.Bl.Impl/"]
COPY ["src/TimeTracking/TimeTracking.Bl.Abstract/TimeTracking.Bl.Abstract.csproj", "src/TimeTracking/TimeTracking.Bl.Abstract/"]
COPY ["src/TimeTracking.Contracts/TimeTracking.Contracts.csproj", "src/TimeTracking.Contracts/"]
RUN dotnet restore "test/IntegrationTests/TimeTracking.IntegrationTests/TimeTracking.IntegrationTests.csproj"
COPY . .
WORKDIR "/src/test/IntegrationTests/TimeTracking.IntegrationTests"
RUN dotnet build "TimeTracking.IntegrationTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeTracking.IntegrationTests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeTracking.IntegrationTests.dll"]