# Test script to verify validation is working
Write-Host "Testing validation implementation..." -ForegroundColor Green

# Test 1: Invalid CPF for Cliente
Write-Host "`nTest 1: Testing invalid CPF validation" -ForegroundColor Yellow
$invalidClienteData = @{
    CpfCnpj = "12345678901"  # Invalid CPF
    TipoCliente = "Físico"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "http://localhost:5000/api/Cliente" -Method POST -Body $invalidClienteData -ContentType "application/json"
    Write-Host "Response: $($response | ConvertTo-Json -Depth 3)" -ForegroundColor Red
} catch {
    Write-Host "Expected validation error: $($_.Exception.Message)" -ForegroundColor Green
}

# Test 2: Invalid barcode for Produto
Write-Host "`nTest 2: Testing invalid barcode validation" -ForegroundColor Yellow
$invalidProdutoData = @{
    codBarras = "123"  # Too short
    nomeProduto = "Test Product"
    descricaoProduto = "Test Description"
    precoCusto = 10.00
    precoVenda = 15.00
    margemLucro = 50.0
    dataFabricao = "2024-01-01"
    dataVencimento = "2024-12-31"
    quatidadeEstoqueProduto = 100
    FornecedorId = "00000000-0000-0000-0000-000000000001"
    CategoriaId = "00000000-0000-0000-0000-000000000001"
    statusAtivo = 1
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "http://localhost:5000/api/Produto" -Method POST -Body $invalidProdutoData -ContentType "application/json"
    Write-Host "Response: $($response | ConvertTo-Json -Depth 3)" -ForegroundColor Red
} catch {
    Write-Host "Expected validation error: $($_.Exception.Message)" -ForegroundColor Green
}

Write-Host "`nValidation tests completed!" -ForegroundColor Green