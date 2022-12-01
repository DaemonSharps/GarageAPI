FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GarageApi/GarageAPI.csproj", "src/GarageApi/"]
COPY ["src/GarageDataBase/GarageDataBase.csproj", "src/GarageDataBase/"]
RUN dotnet restore "src/GarageApi/GarageAPI.csproj"
COPY . .
WORKDIR "/src/src/GarageApi"
RUN dotnet build "GarageAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GarageAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GarageAPI.dll"]
