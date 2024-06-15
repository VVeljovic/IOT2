FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7070

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Analytics/Analytics.csproj", "Analytics/"]
RUN dotnet restore "Analytics/Analytics.csproj"
COPY . .
WORKDIR "/src/Analytics"
RUN dotnet build "Analytics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Analytics.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Analytics.dll"]
