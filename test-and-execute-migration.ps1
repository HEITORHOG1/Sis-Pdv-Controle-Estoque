# Script simples para testar conexão e executar migração
Write-Host "=== TESTE DE CONEXÃO E MIGRAÇÃO SOFT DELETE ===" -ForegroundColor Green
Write-Host ""

# Testar se SQL Server está rodando
Write-Host "Testando conexão com SQL Server..." -ForegroundColor Yellow

try {
    # Tentar conectar usando sqlcmd
    $testResult = sqlcmd -S localhost -d master -E -Q "SELECT 1" 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ SQL Server está rodando!" -ForegroundColor Green
        
        # Verificar se banco pdv_02 existe
        Write-Host "Verificando se banco pdv_02 existe..." -ForegroundColor Yellow
        $dbCheck = sqlcmd -S localhost -d master -E -Q "SELECT name FROM sys.databases WHERE name = 'pdv_02'" -h -1
        
        if ($dbCheck -match "pdv_02") {
            Write-Host "✓ Banco pdv_02 encontrado!" -ForegroundColor Green
            Write-Host ""
            
            # Executar scripts na ordem
            $scripts = @(
                "add-soft-delete-columns.sql",
                "fix-soft-delete-migration.sql", 
                "verify-soft-delete-setup.sql"
            )
            
            foreach ($script in $scripts) {
                if (Test-Path $script) {
                    Write-Host "Executando: $script" -ForegroundColor Cyan
                    $result = sqlcmd -S localhost -d pdv_02 -E -i $script
                    
                    if ($LASTEXITCODE -eq 0) {
                        Write-Host "✓ $script executado com sucesso!" -ForegroundColor Green
                    } else {
                        Write-Host "✗ Erro ao executar $script" -ForegroundColor Red
                        Write-Host $result -ForegroundColor Red
                    }
                    Write-Host ""
                } else {
                    Write-Host "✗ Arquivo não encontrado: $script" -ForegroundColor Red
                }
            }
            
            Write-Host "=== MIGRAÇÃO CONCLUÍDA ===" -ForegroundColor Green
            Write-Host "Verifique os resultados acima para confirmar se tudo foi executado corretamente." -ForegroundColor Yellow
            
        } else {
            Write-Host "✗ Banco pdv_02 não encontrado!" -ForegroundColor Red
            Write-Host "Certifique-se de que o banco existe antes de executar a migração." -ForegroundColor Yellow
        }
        
    } else {
        Write-Host "✗ Não foi possível conectar ao SQL Server!" -ForegroundColor Red
        Write-Host "Verifique se:" -ForegroundColor Yellow
        Write-Host "1. SQL Server está rodando" -ForegroundColor White
        Write-Host "2. Instância está configurada corretamente" -ForegroundColor White
        Write-Host "3. Você tem permissões adequadas" -ForegroundColor White
    }
    
} catch {
    Write-Host "✗ Erro ao testar conexão: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "Pressione Enter para continuar..."
Read-Host