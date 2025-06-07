# KVBlog.Client/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file first
COPY ["kvblog.client.razor/Kvblog.Client.Razor.csproj", "kvblog.client.razor/"]
RUN dotnet restore "kvblog.client.razor/Kvblog.Client.Razor.csproj"

# Copy remaining source
COPY . .
WORKDIR "/src/kvblog.client.razor"
RUN dotnet publish "Kvblog.Client.Razor.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8081
EXPOSE 8081
ENTRYPOINT ["dotnet", "Kvblog.Client.Razor.dll"]
