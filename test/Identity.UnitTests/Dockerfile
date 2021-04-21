#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["test/Identity.UnitTests/Identity.UnitTests.csproj", "test/Identity.UnitTests/"]
COPY ["src/Identity/TimeTracking.Identity.BL.Impl/TimeTracking.Identity.BL.Impl.csproj", "src/Identity/TimeTracking.Identity.BL.Impl/"]
COPY ["src/TimeTracking.Common/TimeTracking.Common.csproj", "src/TimeTracking.Common/"]
COPY ["src/TimeTracking.Templates/TimeTracking.Templates.csproj", "src/TimeTracking.Templates/"]
COPY ["src/Identity/TimeTracking.Identity.BL.Abstract/TimeTracking.Identity.BL.Abstract.csproj", "src/Identity/TimeTracking.Identity.BL.Abstract/"]
COPY ["src/Identity/TimeTracking.Identity.Models/TimeTracking.Identity.Models.csproj", "src/Identity/TimeTracking.Identity.Models/"]
COPY ["src/Identity/TimeTracking.Identity.Entities/TimeTracking.Identity.Entities.csproj", "src/Identity/TimeTracking.Identity.Entities/"]
COPY ["src/Identity/TimeTracking.Identity.Dal.Impl/TimeTracking.Identity.Dal.Impl.csproj", "src/Identity/TimeTracking.Identity.Dal.Impl/"]
COPY ["src/Identity/TimeTracking.Identity.Dal.Abstract/TimeTracking.Identity.Dal.Abstract.csproj", "src/Identity/TimeTracking.Identity.Dal.Abstract/"]
COPY ["src/TimeTracking.Contracts/TimeTracking.Contracts.csproj", "src/TimeTracking.Contracts/"]
RUN dotnet restore "test/Identity.UnitTests/Identity.UnitTests.csproj"
COPY . .
WORKDIR "/src/test/Identity.UnitTests"
RUN dotnet build "Identity.UnitTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.UnitTests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.UnitTests.dll"]