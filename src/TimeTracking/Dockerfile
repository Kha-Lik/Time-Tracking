#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/TimeTracking/TimeTracking.WebApi/TimeTracking.WebApi.csproj", "src/TimeTracking/TimeTracking.WebApi/"]
COPY ["src/TimeTracking/TimeTracking.Bl.Impl/TimeTracking.Bl.Impl.csproj", "src/TimeTracking/TimeTracking.Bl.Impl/"]
COPY ["src/TimeTracking/TimeTracking.Dal.Abstract/TimeTracking.Dal.Abstract.csproj", "src/TimeTracking/TimeTracking.Dal.Abstract/"]
COPY ["src/TimeTracking/TimeTracking.Entities/TimeTracking.Entities.csproj", "src/TimeTracking/TimeTracking.Entities/"]
COPY ["src/TimeTracking.Common/TimeTracking.Common.csproj", "src/TimeTracking.Common/"]
COPY ["src/TimeTracking.Templates/TimeTracking.Templates.csproj", "src/TimeTracking.Templates/"]
COPY ["src/TimeTracking/TimeTracking.Models/TimeTracking.Models.csproj", "src/TimeTracking/TimeTracking.Models/"]
COPY ["src/TimeTracking/TimeTracking.Bl.Abstract/TimeTracking.Bl.Abstract.csproj", "src/TimeTracking/TimeTracking.Bl.Abstract/"]
COPY ["src/TimeTracking.Contracts/TimeTracking.Contracts.csproj", "src/TimeTracking.Contracts/"]
COPY ["src/TimeTracking/TimeTracking.Dal.Impl/TimeTracking.Dal.Impl.csproj", "src/TimeTracking/TimeTracking.Dal.Impl/"]
RUN dotnet restore "src/TimeTracking/TimeTracking.WebApi/TimeTracking.WebApi.csproj"
COPY . .
WORKDIR "/src/src/TimeTracking/TimeTracking.WebApi"
RUN dotnet build "TimeTracking.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeTracking.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeTracking.WebApi.dll"]