# Plano de refatoração – Produto, Estoque, Preços, Compras, Vendas e Financeiro

Objetivo: separar corretamente “cadastro do produto” (dados mestres) de “movimentações/valores” (estoque, compras, vendas e financeiro), corrigir integrações da API e alinhar a UI (WinForms) com as boas práticas.

---

1) Visão de Domínio (alto nível)
- Produto (Catálogo): dados estáveis do item
  - Campos: Id, código de barras, nome, descrição, categoria, fornecedor, status (ativo/inativo), perecível (sim/não)
  - Não armazena quantidade, custo ou preço de venda – isso pertence a módulos específicos
- Estoque (Inventory): saldo controlado por movimentos
  - Movimentos: Entrada (compra, ajuste, devolução de cliente), Saída (venda, perda, transferência)
  - Para perecíveis: lote e validade no movimento (não no cadastro)
  - Saldo atual = somatório dos movimentos (opcionalmente materializado para performance)
- Preços (Pricing): política de preços
  - Tabelas de preço, vigência, regras de margem
  - Custo é originado pelo recebimento de compras; preço de venda fica neste módulo
- Compras (AP): pedidos/notas de compra (Contas a Pagar)
  - No recebimento: cria movimento de estoque (Entrada) + título em AP + atualização de custo médio
- Vendas/PDV (AR): pedidos/vendas/cupons (Contas a Receber/Caixa)
  - Na finalização: cria movimento de estoque (Saída) + título em AR/baixa no caixa
- Financeiro: AP/AR/Caixa/Conciliação
  - Integra com Compras/Vendas; não deve ficar acoplado ao cadastro de produto

2) Problemas atuais e correções imediatas
- Tela de Produto mistura cadastro com quantidade, custo e preço – separar responsabilidades
- Endpoint de listagem incorreto (404). Correção realizada:
  - GET /api/v1/produto/paginated com parser para o nó data.items
- Validações de datas (perecível) fracas
  - Ajustado parse estrito pt-BR (dd/MM/yyyy) e mensagens claras

3) API – proposta de endpoints (REST, versionados)
- Produto
  - POST   /api/v1/produto
  - PUT    /api/v1/produto/{id}
  - DELETE /api/v1/produto/{id}
  - GET    /api/v1/produto/paginated?page=&pageSize=&search=
- Inventory
  - POST   /api/v1/inventory/movements           (cria movimento IN/OUT)
  - GET    /api/v1/inventory/stock?productId=    (saldo por produto)
  - GET    /api/v1/inventory/movements           (consulta movimentos)
  - Extras existentes: alerts, validate-stock (já no projeto)
- Pricing (futuro)
  - CRUD de preços e listas (ex.: /api/v1/prices e /api/v1/price-lists)
- Compras/Vendas
  - A partir dos processos de pedido/nota/finalização, disparam movimentos no estoque e integrações financeiras

4) Banco de Dados/Migrações
- Garantir tabela de StockMovements (já mapeada – MapStockMovement)
- Incluir tabelas para Price/PriceList (separadas de Produto)
- Índices: ProductId, DataMovimento, Lote
- (Opcional) Visão/materialização de saldo para performance

5) WinForms – ajustes de UI/UX
- frmProduto (Cadastro):
  - Remover/ocultar Quantidade, Preço de Custo e Preço de Venda
  - Manter: código de barras, nome, descrição, categoria, fornecedor, perecível, status
  - Validações: código de barras (8–20 dígitos), datas estritas para perecíveis
- Nova tela “Movimentações de Estoque”
  - Entrada/Saída, quantidade, lote, validade (se perecível), observação
  - Integra com POST /inventory/movements
- Tela “Preços” (futuro)
  - Definição/edição de preço vigente por produto e tabela
- PDV
  - Já realiza a saída; integrar com InventoryController (validate-stock e registrar saída efetiva)

6) Services (cliente – Form)
- ProdutoService
  - Listar: usar GET /api/v1/produto/paginated (já corrigido)
  - Adicionar/Alterar/Remover: alinhar para rotas REST acima quando disponíveis no backend
- InventoryService (novo)
  - CriarMovimentoAsync, ListarMovimentosAsync, ObterSaldoAsync, ValidarEstoqueAsync
- PricingService (novo, futuro)
  - CRUD de preços/listas

7) Regras/Validações
- Cadastro de produto não exige quantidade/datas (apenas perecível = sim/não)
- Movimentos de estoque exigem quantidade e, se perecível, lote/validade
- Custo vem de Compras (recebimento); preço de venda do Pricing
- Datas em pt-BR (dd/MM/yyyy) com parse estrito e validações de negócio

8) Observabilidade e erros (cliente+API)
- Padronizar respostas (success, message, data, notifications)
- Logging/métricas por endpoint (tempo, 4xx/5xx)
- Tratamento específico de 401, 404, 422 no cliente com mensagens de negócio

9) Plano de migração (por fases)
- Fase 1 (Estabilização)
  - Corrigir listagem de produtos (feito)
  - Fortalecer validações na tela (feito – perecível/datas, código de barras)
  - Ocultar campos de quantidade/custos/preços no cadastro (UI)
- Fase 2 (Estoque)
  - Implementar endpoints de movimentos no backend (se necessário)
  - Criar tela de Movimentação de Estoque
  - Ajustar PDV para registrar saída via InventoryController
- Fase 3 (Compras/Preços)
  - Recebimento de compras gera movimento IN + custo
  - Módulo de preços (definições e tabelas)
- Fase 4 (Financeiro)
  - Integrações AP/AR, conciliação e relatórios

10) Tarefas (to-do)
- Backend
  - [ ] Confirmar/expor endpoints de Produto (POST/PUT/DELETE)
  - [ ] Expor POST /inventory/movements e GET /inventory/stock
  - [ ] Criar entidades/rotas de Pricing (opcional nesta etapa)
- Frontend (WinForms)
  - [ ] Limpar frmProduto de campos que não são de cadastro e ajustar binding
  - [ ] Criar tela de Movimentação de Estoque (entrada/saída)
  - [ ] Criar InventoryService cliente
  - [ ] Ajustar PDV para usar ValidateStock e registrar saída
- Dados/Infra
  - [ ] Garantir migrações/índices de StockMovements
  - [ ] Tabelas de preço (quando aplicável)

11) Notas adicionais
- O endpoint atual disponível para listagem é GET /api/v1/produto/paginated
- As demais rotas de Produto/Estoque devem ser alinhadas no backend; até lá, mantenha somente leitura no cadastro
- Para produtos perecíveis, a validade deve ser controlada por lote no movimento, permitindo múltiplos lotes por produto

---

Este documento será atualizado conforme os endpoints forem expostos/confirmados e as telas forem evoluídas.