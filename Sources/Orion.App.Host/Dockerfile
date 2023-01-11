FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Sources/Orion.App.Host/Orion.App.Host/Orion.App.Host.csproj", "Sources/Orion.App.Host/Orion.App.Host/"]
RUN dotnet restore "Sources/Orion.App.Host/Orion.App.Host/Orion.App.Host.csproj"
COPY . .
WORKDIR "/src/Sources/Orion.App.Host/Orion.App.Host"
RUN dotnet build "Orion.App.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orion.App.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orion.App.Host.dll"]
