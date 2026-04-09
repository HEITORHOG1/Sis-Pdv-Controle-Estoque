# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Sis.Pdv.Blazor/Sis.Pdv.Blazor.csproj", "Sis.Pdv.Blazor/"]
RUN dotnet restore "Sis.Pdv.Blazor/Sis.Pdv.Blazor.csproj"

COPY . .
WORKDIR "/src/Sis.Pdv.Blazor"
RUN dotnet publish "Sis.Pdv.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8090

ENV ASPNETCORE_URLS=http://+:8090
ENV ASPNETCORE_ENVIRONMENT=Docker

ENTRYPOINT ["dotnet", "Sis.Pdv.Blazor.dll"]
