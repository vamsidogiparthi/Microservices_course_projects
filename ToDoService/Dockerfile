FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGUARTION=Release
WORKDIR /src
COPY ["TodoApi/TodoApi.csproj", "TodoApi/"]
RUN dotnet restore "./TodoApi/./TodoApi.csproj"
COPY . .
WORKDIR "/src/TodoApi"
RUN dotnet build "./TodoApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGUARTION=Release
RUN dotnet publish "./TodoApi.csproj" -c $BUILD_CONFIGURATION -c $BUILD_CONFIGURATION -o /app/publish /p:UseApp

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]