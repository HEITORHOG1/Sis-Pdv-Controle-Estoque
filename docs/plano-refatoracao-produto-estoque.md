# Plano de refatora��o � Produto, Estoque, Pre�os, Compras, Vendas e Financeiro

Objetivo: separar corretamente �cadastro do produto� (dados mestres) de �movimenta��es/valores� (estoque, compras, vendas e financeiro), corrigir integra��es da API e alinhar a UI (WinForms) com as boas pr�ticas.

---

1) Vis�o de Dom�nio (alto n�vel)
- Produto (Cat�logo): dados est�veis do item
  - Campos: Id, c�digo de barras, nome, descri��o, categoria, fornecedor, status (ativo/inativo), perec�vel (sim/n�o)
  - N�o armazena quantidade, custo ou pre�o de venda � isso pertence a m�dulos espec�ficos
- Estoque (Inventory): saldo controlado por movimentos
  - Movimentos: Entrada (compra, ajuste, devolu��o de cliente), Sa�da (venda, perda, transfer�ncia)
  - Para perec�veis: lote e validade no movimento (n�o no cadastro)
  - Saldo atual = somat�rio dos movimentos (opcionalmente materializado para performance)
- Pre�os (Pricing): pol�tica de pre�os
  - Tabelas de pre�o, vig�ncia, regras de margem
  - Custo � originado pelo recebimento de compras; pre�o de venda fica neste m�dulo
- Compras (AP): pedidos/notas de compra (Contas a Pagar)
  - No recebimento: cria movimento de estoque (Entrada) + t�tulo em AP + atualiza��o de custo m�dio
- Vendas/PDV (AR): pedidos/vendas/cupons (Contas a Receber/Caixa)
  - Na finaliza��o: cria movimento de estoque (Sa�da) + t�tulo em AR/baixa no caixa
- Financeiro: AP/AR/Caixa/Concilia��o
  - Integra com Compras/Vendas; n�o deve ficar acoplado ao cadastro de produto

2) Problemas atuais e corre��es imediatas
- Tela de Produto mistura cadastro com quantidade, custo e pre�o � separar responsabilidades
- Endpoint de listagem incorreto (404). Corre��o realizada:
  - GET /api/v1/produto/paginated com parser para o n� data.items
- Valida��es de datas (perec�vel) fracas
  - Ajustado parse estrito pt-BR (dd/MM/yyyy) e mensagens claras

3) API � proposta de endpoints (REST, versionados)
- Produto
  - POST   /api/v1/produto
  - PUT    /api/v1/produto/{id}
  - DELETE /api/v1/produto/{id}
  - GET    /api/v1/produto/paginated?page=&pageSize=&search=
- Inventory
  - POST   /api/v1/inventory/movements           (cria movimento IN/OUT)
  - GET    /api/v1/inventory/stock?productId=    (saldo por produto)
  - GET    /api/v1/inventory/movements           (consulta movimentos)
  - Extras existentes: alerts, validate-stock (j� no projeto)
- Pricing (futuro)
  - CRUD de pre�os e listas (ex.: /api/v1/prices e /api/v1/price-lists)
- Compras/Vendas
  - A partir dos processos de pedido/nota/finaliza��o, disparam movimentos no estoque e integra��es financeiras

4) Banco de Dados/Migra��es
- Garantir tabela de StockMovements (j� mapeada � MapStockMovement)
- Incluir tabelas para Price/PriceList (separadas de Produto)
- �ndices: ProductId, DataMovimento, Lote
- (Opcional) Vis�o/materializa��o de saldo para performance

5) WinForms � ajustes de UI/UX
- frmProduto (Cadastro):
  - Remover/ocultar Quantidade, Pre�o de Custo e Pre�o de Venda
  - Manter: c�digo de barras, nome, descri��o, categoria, fornecedor, perec�vel, status
  - Valida��es: c�digo de barras (8�20 d�gitos), datas estritas para perec�veis
- Nova tela �Movimenta��es de Estoque�
  - Entrada/Sa�da, quantidade, lote, validade (se perec�vel), observa��o
  - Integra com POST /inventory/movements
- Tela �Pre�os� (futuro)
  - Defini��o/edi��o de pre�o vigente por produto e tabela
- PDV
  - J� realiza a sa�da; integrar com InventoryController (validate-stock e registrar sa�da efetiva)

6) Services (cliente � Form)
- ProdutoService
  - Listar: usar GET /api/v1/produto/paginated (j� corrigido)
  - Adicionar/Alterar/Remover: alinhar para rotas REST acima quando dispon�veis no backend
- InventoryService (novo)
  - CriarMovimentoAsync, ListarMovimentosAsync, ObterSaldoAsync, ValidarEstoqueAsync
- PricingService (novo, futuro)
  - CRUD de pre�os/listas

7) Regras/Valida��es
- Cadastro de produto n�o exige quantidade/datas (apenas perec�vel = sim/n�o)
- Movimentos de estoque exigem quantidade e, se perec�vel, lote/validade
- Custo vem de Compras (recebimento); pre�o de venda do Pricing
- Datas em pt-BR (dd/MM/yyyy) com parse estrito e valida��es de neg�cio

8) Observabilidade e erros (cliente+API)
- Padronizar respostas (success, message, data, notifications)
- Logging/m�tricas por endpoint (tempo, 4xx/5xx)
- Tratamento espec�fico de 401, 404, 422 no cliente com mensagens de neg�cio

9) Plano de migra��o (por fases)
- Fase 1 (Estabiliza��o)
  - Corrigir listagem de produtos (feito)
  - Fortalecer valida��es na tela (feito � perec�vel/datas, c�digo de barras)
  - Ocultar campos de quantidade/custos/pre�os no cadastro (UI)
- Fase 2 (Estoque)
  - Implementar endpoints de movimentos no backend (se necess�rio)
  - Criar tela de Movimenta��o de Estoque
  - Ajustar PDV para registrar sa�da via InventoryController
- Fase 3 (Compras/Pre�os)
  - Recebimento de compras gera movimento IN + custo
  - M�dulo de pre�os (defini��es e tabelas)
- Fase 4 (Financeiro)
  - Integra��es AP/AR, concilia��o e relat�rios

10) Tarefas (to-do)
- Backend
  - [ ] Confirmar/expor endpoints de Produto (POST/PUT/DELETE)
  - [ ] Expor POST /inventory/movements e GET /inventory/stock
  - [ ] Criar entidades/rotas de Pricing (opcional nesta etapa)
- Frontend (WinForms)
  - [ ] Limpar frmProduto de campos que n�o s�o de cadastro e ajustar binding
  - [ ] Criar tela de Movimenta��o de Estoque (entrada/sa�da)
  - [ ] Criar InventoryService cliente
  - [ ] Ajustar PDV para usar ValidateStock e registrar sa�da
- Dados/Infra
  - [ ] Garantir migra��es/�ndices de StockMovements
  - [ ] Tabelas de pre�o (quando aplic�vel)

11) Notas adicionais
- O endpoint atual dispon�vel para listagem � GET /api/v1/produto/paginated
- As demais rotas de Produto/Estoque devem ser alinhadas no backend; at� l�, mantenha somente leitura no cadastro
- Para produtos perec�veis, a validade deve ser controlada por lote no movimento, permitindo m�ltiplos lotes por produto

---

Este documento ser� atualizado conforme os endpoints forem expostos/confirmados e as telas forem evolu�das.