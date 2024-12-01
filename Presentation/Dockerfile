FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PeachCrate/PeachCrate.csproj", "PeachCrate/"]
COPY ["DataLayer/DataLayer.csproj", "DataLayer/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["ServiceLayer/ServiceLayer.csproj", "ServiceLayer/"]
RUN dotnet restore "PeachCrate/PeachCrate.csproj"
COPY . .
WORKDIR "/src/PeachCrate"
RUN dotnet build "PeachCrate.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PeachCrate.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PeachCrate.dll"]
