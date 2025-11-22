# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ParqueoUPDS.csproj", "./"]
RUN dotnet restore "ParqueoUPDS.csproj"
COPY . .
RUN dotnet publish "ParqueoUPDS.csproj" -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 1000
ENTRYPOINT ["dotnet", "ParqueoUPDS.dll"]
