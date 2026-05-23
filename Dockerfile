FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/VoguMap.Web/*.csproj", "src/VoguMap.Web/"]
COPY ["src/VoguMap.Domain/*.csproj", "src/VoguMap.Domain/"]
COPY ["src/VoguMap.Application/*.csproj", "src/VoguMap.Application/"]
COPY ["src/VoguMap.Infrastructure/*.csproj", "src/VoguMap.Infrastructure/"]

RUN dotnet restore src/VoguMap.Web/VoguMap.Web.csproj

COPY . .

RUN dotnet publish src/VoguMap.Web/VoguMap.Web.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 80
ENTRYPOINT ["dotnet", "VoguMap.Web.dll"]