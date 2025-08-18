# ?? Correção de Erro JSON - Departamento CRUD

## ?? Problema Identificado

**Erro:** `Invalid pattern ^[a-zA-Z0-9\s\-\_\.]+$ at offset 17. Unrecognized escape sequence \_`

### ?? Análise do Erro

O erro estava ocorrendo durante a deserialização JSON devido ao uso de **RegularExpression** no Data Annotations do `DepartamentoDto`. O padrão regex `^[a-zA-Z0-9\s\-\_\.]+$` continha uma sequência de escape inválida (`\_`) que estava causando problemas na serialização/deserialização JSON.

## ??? Correções Implementadas

### 1. **DepartamentoDto.cs - Removido RegularExpression problemático**

**ANTES:**
```csharp
[RegularExpression(@"^[a-zA-Z0-9\s\-\_\.]+$", ErrorMessage = "Nome deve conter apenas letras, números, espaços e caracteres especiais básicos")]
public string NomeDepartamento { get; set; } = string.Empty;
```

**DEPOIS:**
```csharp
[Required(ErrorMessage = "Nome do departamento é obrigatório")]
[StringLength(150, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 150 caracteres")]
public string NomeDepartamento { get; set; } = string.Empty;
```

**? Solução:** Mantida a validação no método `Validar()` usando LINQ ao invés de Regex problemático:

```csharp
if (nome.Any(c => !char.IsLetterOrDigit(c) && c != ' ' && c != '-' && c != '_' && c != '.'))
{
    erros.Add("Nome deve conter apenas letras, números, espaços e caracteres especiais básicos (-, _, .)");
}
```

### 2. **HttpClientExtensions.cs - Melhorado tratamento de erros JSON**

**Melhorias implementadas:**
- ? Configuração JSON mais robusta com `JsonSerializerOptions`
- ? Tratamento específico para `JsonException`
- ? Logs de debugging para facilitar troubleshooting
- ? Encoding UTF-8 explícito para evitar problemas de charset
- ? Validação de resposta vazia

```csharp
private static readonly JsonSerializerOptions defaultJsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = false,
    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
};
```

### 3. **DepartamentoService.cs - Tratamento de exceções melhorado**

**Adicionado:**
- ? Try-catch específico para deserialização
- ? Logs de debugging detalhados
- ? Tratamento hierárquico de exceções
- ? Mensagens de erro mais informativas

```csharp
try
{
    var result = await response.ReadContentAs<DepartamentoResponse>();
    return result ?? new DepartamentoResponse { success = false, notifications = new List<object> { "Resposta vazia da API" } };
}
catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"Erro ao deserializar resposta: {ex.Message}");
    throw new Exception($"Erro ao processar resposta da API: {ex.Message}");
}
```

## ?? Benefícios da Correção

### ? **Robustez Aumentada**
- Tratamento específico para diferentes tipos de erro JSON
- Logs detalhados para debugging
- Validação alternativa sem dependência de Regex problemático

### ? **Melhor Experiência do Usuário**
- Mensagens de erro mais claras e específicas
- Não mais crashes por problemas de deserialização
- Feedback adequado para diferentes cenários de erro

### ? **Facilidade de Manutenção**
- Logs de debugging para identificar problemas rapidamente
- Código mais modular e testável
- Separação clara entre validação e serialização

## ?? Como Testar a Correção

1. **Teste de Cadastro Normal:**
   - Digite "Frente de Loja" no campo departamento
   - Clique em "Cadastrar"
   - ? Deve funcionar sem erros

2. **Teste de Caracteres Especiais:**
   - Digite "TI-Suporte_2024" 
   - ? Deve aceitar e normalizar para "Ti-Suporte_2024"

3. **Teste de Validação:**
   - Digite apenas "A" (muito curto)
   - ? Deve mostrar erro de validação adequado

4. **Teste de Duplicata:**
   - Tente cadastrar um departamento já existente
   - ? Deve mostrar erro de duplicata

## ?? Logs para Monitoramento

O sistema agora gera logs detalhados em:
- **Debug Output:** Para desenvolvimento
- **Arquivo Local:** Para análise posterior
- **Console:** Para monitoramento em tempo real

**Exemplo de log:**
```
[2024-01-15 10:30:15.123] [INFO] [Cadastro] Departamento 'Frente De Loja' inserido com sucesso!
[2024-01-15 10:30:15.124] [API_SUCCESS] API Call - POST AdicionarDepartamento - 250ms
```

## ?? Status Final

? **Erro JSON Corrigido**  
? **Build Compilando com Sucesso**  
? **Validações Funcionando**  
? **Logs Implementados**  
? **Tratamento de Erros Robusto**  

---

**Próximo Passo:** Teste o cadastro de departamento novamente. O erro de JSON não deve mais ocorrer e você deve receber feedback claro sobre o resultado da operação.

*Correção implementada seguindo as melhores práticas de tratamento de erros em .NET 8 e System.Text.Json.*