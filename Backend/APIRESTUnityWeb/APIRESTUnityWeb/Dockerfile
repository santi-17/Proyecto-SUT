# Etapa base: runtime de ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa build: SDK de .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos el proyecto
COPY . .

# Publicamos la app
RUN dotnet publish -c Release -o /app

# Etapa final: solo runtime + app publicada
FROM base AS final
WORKDIR /app
COPY --from=build /app .

# Comando de inicio
ENTRYPOINT ["dotnet", "APIRESTUnityWeb.dll"]
