# Implementation Plan

- [x] 1. Refatorar modelo Produto e criar entidades de Inventory









  - Remover campos não relacionados a dados mestres do modelo Produto (PrecoCusto, PrecoVenda, MargemLucro, DataFabricao, DataVencimento, QuatidadeEstoqueProduto, MinimumStock, MaximumStock, ReorderPoint, Location)
  - Adicionar campo IsPerecivel ao modelo Produto
  - Criar modelo InventoryBalance para controlar saldos de estoque
  - Criar modelo StockMovementDetail para lotes e validades
  - Atualizar mapeamentos do Entity Framework para as novas estruturas
  - Criar migrações para alterações no banco de dados
  - _Requirements: 1.1, 1.2, 2.1, 6.1, 6.2_

- [x] 2. Implementar sistema de cálculo de saldo de estoque baseado em movimentações





  - Criar serviço InventoryBalanceService para calcular saldos atuais
  - Implementar método para atualizar saldos baseado em StockMovements
  - Criar job/processo para materializar saldos para performance
  - Implementar validações de estoque negativo e regras de negócio
  - Adicionar suporte a lotes e validades para produtos perecíveis
  - Criar índices otimizados para consultas de saldo
  - _Requirements: 2.1, 2.2, 2.3, 6.2, 6.4_

- [x] 3. Expandir e padronizar API REST para Produtos e Inventory








  - Implementar endpoints REST completos para Produto (POST, PUT, DELETE)
  - Melhorar endpoints existentes de Inventory com validações robustas
  - Padronizar respostas da API seguindo padrão ApiResponse<T>
  - Implementar tratamento de erros específicos (400, 401, 403, 404, 409, 422)
  - Adicionar validações de entrada e regras de negócio nos controllers
  - Implementar logging estruturado e métricas para todos os endpoints
  - _Requirements: 3.1, 3.2, 3.3, 3.4, 8.1, 8.2_

- [ ] 4. Refatorar interface WinForms para separação de responsabilidades
  - Limpar frmProduto removendo campos de quantidade, custo e preço
  - Ajustar binding e validações para focar apenas em dados mestres
  - Implementar validação rigorosa de código de barras (8-20 dígitos)
  - Adicionar campo IsPerecivel com validação apropriada
  - Melhorar validações de datas em formato pt-BR (dd/MM/yyyy)
  - Implementar feedback visual claro para validações e erros
  - _Requirements: 4.1, 4.4, 7.1, 8.3_

- [ ] 5. Criar nova interface de Movimentações de Estoque
  - Desenvolver frmMovimentacaoEstoque para entrada e saída de produtos
  - Implementar campos para tipo de movimento, quantidade, lote e validade
  - Adicionar validações específicas para produtos perecíveis (lote/validade obrigatórios)
  - Integrar com InventoryController para criar movimentações
  - Implementar consulta e histórico de movimentações com filtros
  - Adicionar alertas visuais para produtos com estoque baixo
  - _Requirements: 4.2, 2.2, 2.4, 7.2_

- [ ] 6. Implementar e refatorar Services do cliente (WinForms)
  - Refatorar ProdutoService para focar apenas em operações de dados mestres
  - Criar InventoryService completo com todas as operações de estoque
  - Implementar tratamento robusto de erros HTTP com mensagens específicas
  - Adicionar retry logic e timeout para chamadas de API
  - Implementar cache local para melhorar performance de consultas frequentes
  - Padronizar logging e telemetria nos services
  - _Requirements: 5.1, 5.2, 5.3, 5.4, 8.3_

- [ ] 7. Integrar PDV com novo sistema de Inventory
  - Atualizar fluxo de vendas para usar InventoryService.ValidarEstoqueAsync
  - Implementar criação automática de movimentações de saída no PDV
  - Adicionar validação em tempo real de disponibilidade de estoque
  - Implementar tratamento de produtos perecíveis no PDV (seleção de lote)
  - Adicionar alertas de estoque baixo durante o processo de venda
  - Garantir transações atômicas entre venda e movimentação de estoque
  - _Requirements: 4.3, 2.4, 7.2, 9.4_

- [ ] 8. Criar scripts de migração de dados e deploy
  - Desenvolver scripts SQL para migração segura dos dados existentes
  - Criar backup automático antes da migração
  - Implementar migração de saldos atuais para InventoryBalance
  - Criar movimentações históricas iniciais baseadas nos dados atuais
  - Implementar validação de integridade pós-migração
  - Criar scripts de rollback para emergências
  - _Requirements: 6.3, 9.1, 9.2, 9.3, 10.3_

- [ ] 9. Implementar testes abrangentes e validação de qualidade
  - Criar testes unitários para novos modelos e regras de negócio
  - Implementar testes de integração para APIs refatoradas
  - Desenvolver testes end-to-end para fluxos críticos (cadastro, movimentação, PDV)
  - Criar testes de performance para consultas de saldo e movimentações
  - Implementar testes de migração de dados
  - Adicionar testes de UI para novas telas e validações
  - _Requirements: 8.4, 9.4, 10.4_

- [ ] 10. Finalizar observabilidade, documentação e deploy
  - Implementar métricas específicas para operações de estoque
  - Adicionar dashboards para monitoramento de performance
  - Criar documentação técnica completa da nova arquitetura
  - Implementar health checks específicos para integridade de dados
  - Configurar alertas para problemas críticos (inconsistências de estoque)
  - Preparar ambiente de staging para testes finais
  - Executar deploy gradual com feature flags
  - _Requirements: 8.1, 8.2, 9.1, 9.2, 10.1, 10.2_