FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Celsia.TechVisits.Api/Celsia.TechVisits.Api.csproj", "Celsia.TechVisits.Api/"]
RUN dotnet restore "Celsia.TechVisits.Api/Celsia.TechVisits.Api.csproj"
COPY . .
WORKDIR "/src/Celsia.TechVisits.Api"
RUN dotnet build "Celsia.TechVisits.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Celsia.TechVisits.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "Celsia.TechVisits.Api.dll"]
