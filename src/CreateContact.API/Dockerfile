# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CreateContact.API/CreateContact.API.csproj", "src/CreateContact.API/"]
COPY ["src/CreateContact.Application/CreateContact.Application.csproj", "src/CreateContact.Application/"]
COPY ["src/CreateContact.Domain/CreateContact.Domain.csproj", "src/CreateContact.Domain/"]
COPY ["src/CreateContact.Infrastructure/CreateContact.Infrastructure.csproj", "src/CreateContact.Infrastructure/"]
RUN dotnet restore "./src/CreateContact.API/CreateContact.API.csproj"
COPY . .
WORKDIR "/src/src/CreateContact.API"
RUN dotnet build "./CreateContact.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CreateContact.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CreateContact.API.dll"]