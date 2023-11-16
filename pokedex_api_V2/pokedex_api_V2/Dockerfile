#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["pokedex_api_V2/pokedex_api_V2.csproj", "pokedex_api_V2/"]
RUN dotnet restore "pokedex_api_V2/pokedex_api_V2.csproj"
COPY . .
WORKDIR "/src/pokedex_api_V2"
RUN dotnet build "pokedex_api_V2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pokedex_api_V2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pokedex_api_V2.dll"]