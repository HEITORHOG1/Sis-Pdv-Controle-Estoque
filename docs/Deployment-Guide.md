# Guia de Deploy do Sistema PDV

## Visão Geral

Este guia cobre o deploy do sistema PDV (Ponto de Venda) nos ambientes de desenvolvimento, staging e produção. O projeto suporta deploy via **Docker Compose** (recomendado) ou **manual** com `dotnet publish`.

## Pré-Requisitos

### Stack Tecnológica

| Componente       | Versão Mínima     | Uso                                    |
|------------------|-------------------|----------------------------------------|
| .NET SDK         | 8.0               | Build e runtime da API                 |
| MySQL            | 8.0               | Banco de dados relacional              |
| RabbitMQ         | 3.12+             | Fila de mensagens (cupons fiscais)     |
| Docker           | 24.0+             | Containerização (opcional)             |
| Docker Compose   | 2.20+             | Orquestração de containers             |
| Nginx            | alpine             | Reverse proxy e terminação SSL         |

### Requisitos de Hardware (Produção)

| Recurso  | Mínimo | Recomendado |
|----------|--------|-------------|
| CPU      | 2 cores| 4 cores     |
| RAM      | 2 GB   | 4 GB+       |
| Disco    | 10 GB  | 50 GB+      |

## Estrutura dos Arquivos de Deploy

```
raiz/
├── Dockerfile                      ← Build multi-stage para produção
├── Dockerfile.development          ← Build para desenvolvimento com hot reload
├── docker-compose.yml              ← Produção (4 serviços)
├── docker-compose.development.yml  ← Desenvolvimento local
├── docker-compose.staging.yml      ← Staging/pré-produção
├── .env.example                    ← Template de variáveis de ambiente
├── nginx/
│   └── nginx.conf                  ← Configuração do reverse proxy
└── dev-run.ps1                     ← Script PowerShell para rodar localmente
```

## Deploy Local (Desenvolvimento)

### Opção 1: Sem Docker

```powershell
# 1. Clone o repositório
git clone <repository-url>
cd Sis-Pdv-Controle-Estoque

# 2. Restaure as dependências
dotnet restore

# 3. Configure os User Secrets (credenciais do banco)
cd Sis-Pdv-Controle-Estoque-API
dotnet user-secrets set "ConnectionStrings:DatabasePassword" "sua_senha"
dotnet user-secrets set "Authentication:JwtSecret" "chave_jwt_minimo_32_caracteres_aqui"
cd ..

# 4. Compile a solução
dotnet build

# 5. Aplique as migrações do banco
dotnet ef database update --project Sis-Pdv-Controle-Estoque-Infra --startup-project Sis-Pdv-Controle-Estoque-API

# 6. Execute a API
dotnet run --project Sis-Pdv-Controle-Estoque-API
```

A API estará disponível em `http://localhost:7003`. O Swagger UI estará em `http://localhost:7003/api-docs`.

> **Pré-requisito:** MySQL 8.0 e RabbitMQ devem estar rodando localmente.

### Opção 2: Com Docker Compose (Desenvolvimento)

```bash
# Copia o template de variáveis
cp .env.example .env
# Edite o .env com suas credenciais

# Sobe todos os serviços com hot reload
docker-compose -f docker-compose.development.yml up
```

## Deploy com Docker Compose (Produção)

### Arquitetura dos Containers

```
┌─────────────────────────────────────────────────────┐
│                    Rede: pdv-network                 │
│                                                     │
│  ┌────────────┐    ┌────────────┐    ┌───────────┐  │
│  │   Nginx    │───▶│  PDV API   │───▶│   MySQL   │  │
│  │  :80/:443  │    │   :8080    │    │   :3306   │  │
│  └────────────┘    └──────┬─────┘    └───────────┘  │
│                           │                         │
│                    ┌──────▼─────┐                    │
│                    │  RabbitMQ  │                    │
│                    │ :5672/:15672│                    │
│                    └────────────┘                    │
└─────────────────────────────────────────────────────┘
```

### Passo a Passo

```bash
# 1. Crie o arquivo de variáveis de ambiente
cp .env.example .env

# 2. Edite o .env com valores seguros para produção
#    - DB_PASSWORD: senha forte para o banco
#    - MYSQL_ROOT_PASSWORD: senha do root do MySQL
#    - RABBITMQ_PASSWORD: senha do RabbitMQ
#    - JWT_SECRET: chave de 32+ caracteres
#    - PDV_ENCRYPTION_KEY: chave de exatamente 32 caracteres

# 3. Suba os containers
docker-compose up -d

# 4. Verifique os logs
docker-compose logs -f pdv-api

# 5. Verifique o health check
curl http://localhost:8080/health
```

### Volumes Persistentes

O `docker-compose.yml` define 4 volumes:

| Volume           | Container  | Caminho no Container    | Propósito              |
|------------------|------------|-------------------------|------------------------|
| `mysql-data`     | pdv-mysql  | `/var/lib/mysql`        | Dados do banco         |
| `rabbitmq-data`  | pdv-rabbitmq| `/var/lib/rabbitmq`    | Estado das filas       |
| `pdv-logs`       | pdv-api    | `/app/logs`             | Logs da aplicação      |
| `pdv-backups`    | pdv-api    | `/app/backups`          | Backups automáticos    |

### Health Checks dos Containers

Todos os containers possuem health checks configurados:

- **pdv-api:** `curl -f http://localhost:8080/health` (intervalo: 30s, timeout: 10s)
- **mysql:** `mysqladmin ping` (start_period: 60s para migrações iniciais)
- **rabbitmq:** `rabbitmq-diagnostics ping`

A API só inicia após MySQL e RabbitMQ estarem healthy (`depends_on: condition: service_healthy`).

## Dockerfile (Multi-Stage Build)

O Dockerfile utiliza 3 estágios otimizados:

```
Estágio 1: build     → SDK 8.0, restore + build
Estágio 2: publish   → Publica em modo Release
Estágio 3: final     → Runtime 8.0 apenas (imagem menor)
```

Características de segurança do Dockerfile:
- ✅ Executa como usuário não-root (`pdvuser`)
- ✅ Diretórios de logs/backups/temp com permissões restritas
- ✅ Health check embutido na imagem
- ✅ `UseAppHost=false` para imagem mais leve

## Deploy Manual (Sem Docker)

### Build e Publicação

```bash
# Build para produção
dotnet publish Sis-Pdv-Controle-Estoque-API/Sis-Pdv-Controle-Estoque-API.csproj \
  -c Release \
  -o /opt/pdv-system/app \
  --self-contained false \
  --runtime linux-x64
```

### Systemd Service (Linux)

Crie `/etc/systemd/system/pdv-system.service`:

```ini
[Unit]
Description=PDV Control System API
After=network.target mysql.service rabbitmq-server.service

[Service]
Type=notify
User=www-data
Group=www-data
WorkingDirectory=/opt/pdv-system/app
ExecStart=/usr/bin/dotnet /opt/pdv-system/app/Sis-Pdv-Controle-Estoque-API.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=pdv-system
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
EnvironmentFile=/opt/pdv-system/.env

[Install]
WantedBy=multi-user.target
```

```bash
sudo systemctl enable pdv-system
sudo systemctl start pdv-system
sudo systemctl status pdv-system
```

## Configuração do Nginx (Reverse Proxy)

```nginx
server {
    listen 80;
    server_name your-domain.com;
    return 301 https://$server_name$request_uri;
}

server {
    listen 443 ssl http2;
    server_name your-domain.com;

    ssl_certificate /etc/ssl/certs/pdv-system.crt;
    ssl_certificate_key /etc/ssl/private/pdv-system.key;
    ssl_protocols TLSv1.2 TLSv1.3;

    # Security headers
    add_header Strict-Transport-Security "max-age=63072000; includeSubDomains; preload";
    add_header X-Content-Type-Options nosniff;
    add_header X-Frame-Options DENY;

    location / {
        proxy_pass http://localhost:8080;
        proxy_http_version 1.1;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

## Banco de Dados

### Setup Inicial

O sistema aplica migrações automaticamente no startup por meio de `db.Database.MigrateAsync()` no `Program.cs`. Não é necessário rodar migrações manualmente.

### Seed de Dados

Na primeira execução (banco vazio), o sistema automaticamente executa:
1. **AuthSeederService** — cria roles padrão (SuperAdmin, Admin, Manager, Cashier, Viewer), permissões e o usuário admin
2. **DomainSeederService** — cria dados de exemplo (categorias, departamentos, fornecedores, produtos, clientes, etc.)

Em execuções subsequentes, apenas garante que o mapeamento admin/roles existe.

### Configuração de Performance do MySQL

```sql
SET GLOBAL innodb_buffer_pool_size = 2147483648;  -- 2GB (ajuste pela RAM)
SET GLOBAL innodb_log_file_size = 268435456;       -- 256MB
SET GLOBAL max_connections = 200;
```

## Certificado SSL/TLS

```bash
# Let's Encrypt (automático)
sudo apt install certbot python3-certbot-nginx
sudo certbot --nginx -d your-domain.com

# Renovação automática via cron
sudo certbot renew --dry-run
```

## Verificação Pós-Deploy

```bash
# 1. Health check básico
curl -f http://localhost:8080/health

# 2. Health check detalhado
curl http://localhost:8080/health-ui

# 3. Swagger UI
curl http://localhost:8080/api-docs

# 4. Logs da aplicação
docker-compose logs -f pdv-api
```

## Procedimento de Atualização

```bash
# 1. Pare o serviço
docker-compose stop pdv-api

# 2. Faça backup
docker-compose exec mysql mysqldump -u root -p PDV_PROD > backup_$(date +%Y%m%d).sql

# 3. Rebuild e suba
docker-compose build pdv-api
docker-compose up -d pdv-api

# 4. Verifique (as migrações são aplicadas automaticamente)
docker-compose logs -f pdv-api
curl -f http://localhost:8080/health
```

## Rollback

```bash
# 1. Pare o container
docker-compose stop pdv-api

# 2. Restaure o banco se necessário
docker-compose exec -T mysql mysql -u root -p PDV_PROD < backup_anterior.sql

# 3. Aponte para a imagem anterior
docker-compose up -d pdv-api
```

## Troubleshooting

| Problema | Causa Provável | Solução |
|----------|----------------|---------|
| API não inicia | Connection string incorreta | Verifique variáveis no `.env` |
| Erro de migração | Schema inconsistente | Verifique arquivos SQL de fix no projeto da API |
| RabbitMQ inacessível | Container não healthy | `docker-compose restart rabbitmq` |
| Timeout no health check | MySQL ainda inicializando | Aguarde `start_period` de 60s |
| 403 no Swagger | CORS não configurado | Adicione origem em `Cors:AllowedOrigins` |

---

Autor: Heitor Gonçalves — https://www.linkedin.com/in/heitorhog/