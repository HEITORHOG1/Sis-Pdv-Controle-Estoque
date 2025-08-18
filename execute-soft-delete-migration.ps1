# Script PowerShell para executar a migração completa do soft delete
param(
    [Parameter(Mandatory=$true)]
    [string]$ConnectionString,
    
    [Parameter(Mandatory=$false)]
    [string]$DatabaseName = "pdv_02"
)

Write-Host "=== MIGRAÇÃO SOFT DELETE ===" -ForegroundColor Green
Write-Host ""

# Função para executar script SQL
function Execute-SqlScript {
    param(
        [string]$ScriptPath,
        [string]$ConnectionString,
        [string]$Description
    )
    
    Write-Host "Executando: $Description" -ForegroundColor Yellow
    Write-Host "Script: $ScriptPath" -ForegroundColor Gray
    
    try {
        if (Test-Path $ScriptPath) {
            $scriptContent = Get-Content $ScriptPath -Raw
            
            # Usar sqlcmd se disponível
            if (Get-Command sqlcmd -ErrorAction SilentlyContinue) {
                $tempFile = [System.IO.Path]::GetTempFileName() + ".sql"
                $scriptContent | Out-File -FilePath $tempFile -Encoding UTF8
                
                sqlcmd -S "localhost" -d $DatabaseName -i $tempFile -b
                
                Remove-Item $tempFile -Force
            }
            else {
                Write-Host "sqlcmd não encontrado. Usando .NET SqlConnection..." -ForegroundColor Yellow
                
                Add-Type -AssemblyName System.Data
                $connection = New-Object System.Data.SqlClient.SqlConnection($ConnectionString)
                $connection.Open()
                
                $command = New-Object System.Data.SqlClient.SqlCommand($scriptContent, $connection)
                $command.CommandTimeout = 300
                $result = $command.ExecuteNonQuery()
                
                $connection.Close()
                Write-Host "Comando executado com sucesso. Linhas afetadas: $result" -ForegroundColor Green
            }
            
            Write-Host "✓ Concluído com sucesso!" -ForegroundColor Green
        }
        else {
            Write-Host "✗ Arquivo não encontrado: $ScriptPath" -ForegroundColor Red
            return $false
        }
    }
    catch {
        Write-Host "✗ Erro ao executar script: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
    
    Write-Host ""
    return $true
}

# Verificar se os arquivos existem
$scripts = @(
    @{ Path = "add-soft-delete-columns.sql"; Description = "Adicionando colunas de soft delete" },
    @{ Path = "fix-soft-delete-migration.sql"; Description = "Corrigindo registros existentes" },
    @{ Path = "verify-soft-delete-setup.sql"; Description = "Verificando implementação" }
)

Write-Host "Verificando arquivos de script..." -ForegroundColor Cyan
$allFilesExist = $true
foreach ($script in $scripts) {
    if (Test-Path $script.Path) {
        Write-Host "✓ $($script.Path)" -ForegroundColor Green
    }
    else {
        Write-Host "✗ $($script.Path) - ARQUIVO NÃO ENCONTRADO" -ForegroundColor Red
        $allFilesExist = $false
    }
}

if (-not $allFilesExist) {
    Write-Host ""
    Write-Host "Alguns arquivos de script não foram encontrados. Abortando execução." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Todos os arquivos encontrados. Iniciando migração..." -ForegroundColor Green
Write-Host ""

# Executar scripts na ordem correta
$success = $true

foreach ($script in $scripts) {
    $result = Execute-SqlScript -ScriptPath $script.Path -ConnectionString $ConnectionString -Description $script.Description
    if (-not $result) {
        $success = $false
        break
    }
}

Write-Host "=== RESULTADO DA MIGRAÇÃO ===" -ForegroundColor Green
if ($success) {
    Write-Host "✓ Migração concluída com sucesso!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Próximos passos:" -ForegroundColor Cyan
    Write-Host "1. Compile e teste a aplicação" -ForegroundColor White
    Write-Host "2. Teste as funcionalidades de exclusão" -ForegroundColor White
    Write-Host "3. Verifique se as consultas estão funcionando corretamente" -ForegroundColor White
    Write-Host "4. Execute os testes automatizados se disponíveis" -ForegroundColor White
}
else {
    Write-Host "✗ Migração falhou!" -ForegroundColor Red
    Write-Host "Verifique os erros acima e corrija antes de continuar." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Migração finalizada."