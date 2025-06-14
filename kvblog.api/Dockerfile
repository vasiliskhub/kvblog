FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy just the project file first
COPY ["kvblog.api/Kvblog.Api.csproj", "kvblog.api/"]
COPY ["kvblog.api.application/Kvblog.Api.Application.csproj", "kvblog.api.application/"]
COPY ["kvblog.api.contracts/Kvblog.Api.Contracts.csproj", "kvblog.api.contracts/"]
COPY ["kvblog.api.db/Kvblog.Api.Db.csproj", "kvblog.api.db/"]
RUN dotnet restore "kvblog.api/Kvblog.Api.csproj"

# Copy everything else
COPY . .

WORKDIR "/src/kvblog.api"
RUN dotnet publish "Kvblog.Api.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Kvblog.Api.dll"]
