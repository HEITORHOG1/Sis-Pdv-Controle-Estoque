# Requirements Document

## Introduction

O sistema atual possui problemas de separação de responsabilidades onde o cadastro de produtos está misturado com movimentações de estoque, preços, compras, vendas e financeiro. Esta refatoração visa separar corretamente os domínios seguindo as melhores práticas de DDD (Domain-Driven Design), criando módulos independentes e bem definidos para Produto (dados mestres), Estoque (movimentações), Preços (políticas de preço), Compras (AP), Vendas (AR) e Financeiro.

## Requirements

### Requirement 1

**User Story:** Como desenvolvedor, eu quero separar o domínio de Produto dos demais módulos, para que o cadastro contenha apenas dados mestres do item sem informações de quantidade, custo ou preço.

#### Acceptance Criteria

1. WHEN um produto é cadastrado THEN o sistema SHALL armazenar apenas dados mestres (Id, código de barras, nome, descrição, categoria, fornecedor, status ativo/inativo, perecível sim/não)
2. WHEN um produto é cadastrado THEN o sistema SHALL NOT armazenar quantidade, custo ou preço de venda no cadastro do produto
3. WHEN um produto perecível é cadastrado THEN o sistema SHALL validar apenas se é perecível, sem exigir datas de validade no cadastro
4. WHEN o código de barras é informado THEN o sistema SHALL validar que possui entre 8-20 dígitos

### Requirement 2

**User Story:** Como usuário do sistema, eu quero um módulo de Estoque independente que controle saldos através de movimentações, para que possa rastrear entradas e saídas de forma precisa.

#### Acceptance Criteria

1. WHEN uma movimentação de estoque é criada THEN o sistema SHALL registrar tipo (Entrada/Saída), quantidade, produto, data e observação
2. WHEN um produto é perecível AND uma movimentação é criada THEN o sistema SHALL exigir lote e data de validade
3. WHEN o saldo atual é consultado THEN o sistema SHALL calcular baseado no somatório das movimentações
4. WHEN uma saída é registrada THEN o sistema SHALL validar se há saldo suficiente disponível

### Requirement 3

**User Story:** Como usuário do sistema, eu quero uma API REST bem estruturada e versionada, para que as integrações sejam consistentes e manteníveis.

#### Acceptance Criteria

1. WHEN endpoints de produto são acessados THEN o sistema SHALL responder via rotas /api/v1/produto com operações CRUD completas
2. WHEN endpoints de estoque são acessados THEN o sistema SHALL responder via rotas /api/v1/inventory para movimentações e consultas de saldo
3. WHEN uma resposta é retornada THEN o sistema SHALL seguir padrão consistente com success, message, data e notifications
4. WHEN erros ocorrem THEN o sistema SHALL retornar códigos HTTP apropriados (401, 404, 422) com mensagens de negócio claras

### Requirement 4

**User Story:** Como usuário da interface WinForms, eu quero telas separadas por responsabilidade, para que cada funcionalidade seja clara e focada em seu propósito.

#### Acceptance Criteria

1. WHEN a tela de cadastro de produto é aberta THEN o sistema SHALL exibir apenas campos de dados mestres (sem quantidade, custo, preço)
2. WHEN uma nova tela de movimentações é criada THEN o sistema SHALL permitir registrar entradas e saídas de estoque
3. WHEN o PDV é utilizado THEN o sistema SHALL integrar com o módulo de estoque para validar e registrar saídas
4. WHEN datas são inseridas THEN o sistema SHALL validar formato pt-BR (dd/MM/yyyy) com parse estrito

### Requirement 5

**User Story:** Como desenvolvedor, eu quero services no cliente bem organizados, para que cada módulo tenha sua responsabilidade específica e as integrações sejam claras.

#### Acceptance Criteria

1. WHEN ProdutoService é utilizado THEN o sistema SHALL focar apenas em operações CRUD de dados mestres
2. WHEN InventoryService é criado THEN o sistema SHALL gerenciar movimentações, saldos e validações de estoque
3. WHEN serviços são chamados THEN o sistema SHALL utilizar endpoints REST apropriados e tratar erros específicos
4. WHEN validações são executadas THEN o sistema SHALL aplicar regras de negócio no cliente antes de enviar para API

### Requirement 6

**User Story:** Como administrador do sistema, eu quero que o banco de dados seja estruturado corretamente, para que suporte a separação de domínios com performance adequada.

#### Acceptance Criteria

1. WHEN tabelas são criadas THEN o sistema SHALL ter estruturas separadas para Produto, StockMovements e futuras tabelas de Price/PriceList
2. WHEN consultas de saldo são executadas THEN o sistema SHALL utilizar índices otimizados em ProductId, DataMovimento e Lote
3. WHEN migrações são aplicadas THEN o sistema SHALL manter compatibilidade com dados existentes
4. WHEN performance é crítica THEN o sistema SHALL considerar materialização de saldos para consultas frequentes

### Requirement 7

**User Story:** Como usuário do sistema, eu quero que as regras de negócio sejam consistentes e bem validadas, para que os dados mantenham integridade e as operações sejam confiáveis.

#### Acceptance Criteria

1. WHEN produtos são cadastrados THEN o sistema SHALL NOT exigir quantidade ou datas (apenas indicar se é perecível)
2. WHEN movimentações são criadas THEN o sistema SHALL exigir quantidade e, para perecíveis, lote e validade obrigatórios
3. WHEN custos são definidos THEN o sistema SHALL originar apenas de recebimentos de compras
4. WHEN preços são definidos THEN o sistema SHALL utilizar módulo específico de Pricing separado do cadastro

### Requirement 8

**User Story:** Como desenvolvedor, eu quero observabilidade e tratamento de erros padronizados, para que problemas sejam identificados rapidamente e usuários recebam feedback claro.

#### Acceptance Criteria

1. WHEN endpoints são acessados THEN o sistema SHALL registrar logs com tempo de resposta e códigos de status
2. WHEN erros 4xx/5xx ocorrem THEN o sistema SHALL registrar métricas e detalhes para análise
3. WHEN erros são tratados no cliente THEN o sistema SHALL exibir mensagens de negócio específicas por tipo de erro
4. WHEN operações são executadas THEN o sistema SHALL fornecer feedback claro sobre sucesso ou falha

### Requirement 9

**User Story:** Como gestor do projeto, eu quero uma migração faseada e controlada, para que a refatoração seja implementada sem impactar operações críticas.

#### Acceptance Criteria

1. WHEN Fase 1 é executada THEN o sistema SHALL estabilizar listagem e validações sem quebrar funcionalidades existentes
2. WHEN Fase 2 é executada THEN o sistema SHALL implementar módulo de estoque mantendo compatibilidade
3. WHEN Fase 3 é executada THEN o sistema SHALL integrar compras e preços de forma incremental
4. WHEN cada fase é concluída THEN o sistema SHALL manter todas as funcionalidades anteriores operacionais

### Requirement 10

**User Story:** Como usuário final, eu quero que o sistema mantenha todas as funcionalidades atuais durante a refatoração, para que não haja interrupção nas operações do negócio.

#### Acceptance Criteria

1. WHEN refatorações são aplicadas THEN o sistema SHALL manter compatibilidade com fluxos existentes de PDV
2. WHEN novos módulos são criados THEN o sistema SHALL integrar sem quebrar funcionalidades de vendas e estoque
3. WHEN dados são migrados THEN o sistema SHALL preservar informações históricas e saldos atuais
4. WHEN testes são executados THEN o sistema SHALL validar que todas as operações críticas continuam funcionando