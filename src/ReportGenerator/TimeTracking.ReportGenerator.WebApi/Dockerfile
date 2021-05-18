#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/ReportGenerator/TimeTracking.ReportGenerator.WebApi/TimeTracking.ReportGenerator.WebApi.csproj", "src/ReportGenerator/TimeTracking.ReportGenerator.WebApi/"]
COPY ["src/ReportGenerator/TimeTracking.ReportGenerator.Bl.Abstract/TimeTracking.ReportGenerator.Bl.Abstract.csproj", "src/ReportGenerator/TimeTracking.ReportGenerator.Bl.Abstract/"]
COPY ["src/ReportGenerator/TimeTracking.ReportGenerator.Models/TimeTracking.ReportGenerator.Models.csproj", "src/ReportGenerator/TimeTracking.ReportGenerator.Models/"]
COPY ["src/TimeTracking.Common/TimeTracking.Common.csproj", "src/TimeTracking.Common/"]
COPY ["src/TimeTracking.Templates/TimeTracking.Templates.csproj", "src/TimeTracking.Templates/"]
COPY ["src/ReportGenerator/TimeTracking.ReportGenerator.Bl.Impl/TimeTracking.ReportGenerator.Bl.Impl.csproj", "src/ReportGenerator/TimeTracking.ReportGenerator.Bl.Impl/"]
COPY ["src/ReportGenerator/TimeTracking.ReportGenerator.Dal.Abstract/TimeTracking.ReportGenerator.Dal.Abstract.csproj", "src/ReportGenerator/TimeTracking.ReportGenerator.Dal.Abstract/"]
RUN dotnet restore "src/ReportGenerator/TimeTracking.ReportGenerator.WebApi/TimeTracking.ReportGenerator.WebApi.csproj"
COPY . .
WORKDIR "/src/src/ReportGenerator/TimeTracking.ReportGenerator.WebApi"
RUN dotnet build "TimeTracking.ReportGenerator.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeTracking.ReportGenerator.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeTracking.ReportGenerator.WebApi.dll"]