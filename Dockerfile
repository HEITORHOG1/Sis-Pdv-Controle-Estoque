# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["Sis-Pdv-Controle-Estoque-API/Sis-Pdv-Controle-Estoque-API.csproj", "Sis-Pdv-Controle-Estoque-API/"]
COPY ["Sis-Pdv-Controle-Estoque/Sis-Pdv-Controle-Estoque-Domain.csproj", "Sis-Pdv-Controle-Estoque/"]
COPY ["Sis-Pdv-Controle-Estoque-Infra/Sis-Pdv-Controle-Estoque-Infra.csproj", "Sis-Pdv-Controle-Estoque-Infra/"]
COPY ["MessageBus/MessageBus.csproj", "MessageBus/"]

# Restore dependencies
RUN dotnet restore "Sis-Pdv-Controle-Estoque-API/Sis-Pdv-Controle-Estoque-API.csproj"

# Copy source code
COPY . .

# Build application
WORKDIR "/src/Sis-Pdv-Controle-Estoque-API"
RUN dotnet build "Sis-Pdv-Controle-Estoque-API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Sis-Pdv-Controle-Estoque-API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user
RUN groupadd -r pdvuser && useradd -r -g pdvuser pdvuser

# Create directories and set permissions
RUN mkdir -p /app/logs /app/backups /app/temp && \
    chown -R pdvuser:pdvuser /app

# Install required packages for health checks and monitoring
RUN apt-get update && apt-get install -y \
    curl \
    && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Set ownership
RUN chown -R pdvuser:pdvuser /app

# Switch to non-root user
USER pdvuser

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Entry point
ENTRYPOINT ["dotnet", "Sis-Pdv-Controle-Estoque-API.dll"]