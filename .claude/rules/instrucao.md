# Instruções do Projeto Sis-Pdv-Controle-Estoque

## Verificação Rápida do Projeto

1. Compile o projeto
```powershell
dotnet build
```

2. Execute os testes
```powershell
dotnet test --logger "console;verbosity=detailed"
```

3. Verifique formatação
```powershell
dotnet format --verify-no-changes
```

## Caso falhe algum passo

- **Build falha**: verifique se `dotnet restore` foi executado
- **Testes falham**: verifique a conexão com o banco de testes
- **Formatação**: execute `dotnet format` para corrigir automaticamente
