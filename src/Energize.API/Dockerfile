FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Energize.API/Energize.API.csproj", "Energize.API/"]
RUN dotnet restore "Energize.API/Energize.API.csproj"
COPY . .
WORKDIR "/src/Energize.API"
RUN dotnet build "Energize.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Energize.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Energize.API.dll"]
