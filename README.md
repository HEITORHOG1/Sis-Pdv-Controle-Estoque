# Sis-Pdv-Controle-Estoque
#  Projeto PDV e cadastro de produtos.
Projeto opensource para todos que desejam aprender sobre um desenvolvimento de um PDV de vendas Basico, com cadastros.

# O PROJETO CONTEM
Categoria,
Cliente,
Colaborador,
Departamento,
Fornecedor,
Pedido,
Produto,
ProdutoPedido,
E MAIS O PDV DE VENDA FRENTE DE LOJA.

# Sis-Pdv-Controle-Estoque

![Logo do Projeto](link-para-o-logo)

> Um sistema robusto de Ponto de Venda (PDV) e gerenciamento de estoque implementado em ASP.NET Core.

## Autor e contato

- Autor: Heitor Gonçalves
- LinkedIn: https://www.linkedin.com/in/heitorhog/

## Sobre o Projeto

Este é um projeto open-source destinado àqueles interessados em aprender sobre o desenvolvimento de um PDV. Utilizamos práticas modernas de desenvolvimento e padrões de arquitetura, como o Domain-Driven Design (DDD) e CQRS (Command Query Responsibility Segregation). A aplicação é construída em ASP.NET Core, integrada a um banco de dados MySQL, e utiliza o RabbitMQ para o processamento assíncrono de mensagens.

### Recursos Principais

- **Gerenciamento de Vendas**: Sistema completo de controle de vendas, suportando transações, processamento de pagamentos e emissão de recibos.
- **Gerenciamento de Estoque**: Ferramentas para gerenciar estoque, permitindo adicionar, modificar, listar e remover itens, e acompanhar os níveis de estoque.
- **Consulta de Estoque**: Consulte facilmente o estoque por ID ou nome do item.
- **Categorização de Produtos**: Organize os produtos em categorias para facilitar o gerenciamento e a navegação.
- **Integração com RabbitMQ**: Processamento assíncrono de mensagens e transações, essencial para envio de cupons fiscais à SEFAZ.

## Arquitetura e Padrões de Projeto

- **Domain-Driven Design (DDD)**: A modelagem rica do domínio de negócio e a segregação de responsabilidades fornecem uma base sólida para o sistema.
- **CQRS**: Separamos as operações de leitura e gravação, melhorando o desempenho e a escalabilidade do sistema.
- **Repository Pattern**: Abstraímos o acesso aos dados, permitindo um acoplamento mais frouxo e maior testabilidade do código.
- **Dependency Injection**: Gerenciamos as dependências entre os objetos, aumentando a flexibilidade e permitindo a substituição e o teste de componentes com mais facilidade.

## Tecnologias e Ferramentas

- **ASP.NET Core**: Framework moderno e de alto desempenho para desenvolvimento de aplicações web e APIs.
- **C#**: Linguagem de programação multi-paradigma e fortemente tipada.
- **MySQL**: Sistema robusto de gerenciamento de banco de dados relacional.
- **Entity Framework**: Ferramenta de mapeamento objeto-relacional (ORM) para .NET.
- **RabbitMQ**: Servidor de mensagens para processamento assíncrono de transações.

## Práticas e Princípios
Princípios SOLID de programação e práticas de Clean Code para garantir um código limpo, manutenível e testável. Nosso código é bem documentado, com foco em manter as funções e classes pequenas e focadas, facilitando a legibilidade e a manutenção do código.

## Diagramas de Arquitetura

Na pasta `Sis-Pdv-Controle-Estoque/Diagrams` há dois diagramas de classe gerados pelo Visual Studio:

- **PDV.cd** – apresenta a visão geral das entidades relacionadas ao ponto de venda.
- **SisPdv.cd** – mostra o relacionamento entre as demais camadas do sistema.

Abra esses arquivos diretamente no Visual Studio para visualizar como as classes se conectam.

### Camadas do Projeto

1. **Domínio** (`Sis-Pdv-Controle-Estoque`): modelos de domínio, comandos e interfaces.
2. **Infraestrutura** (`Sis-Pdv-Controle-Estoque-Infra`): repositórios e acesso ao banco de dados.
3. **API** (`Sis-Pdv-Controle-Estoque-API`): serviços HTTP e integração com RabbitMQ.
4. **Interface Gráfica** (`Sis-Pdv-Controle-Estoque-Form`): aplicação desktop (WinForms) utilizada no PDV.

## Configuração do Banco de Dados

1. Abra o arquivo `appsettings.json`
2. Configure a string de conexão do banco de dados no campo `ControleFluxoCaixaConnectionString`

Exemplo da string de conexão:
```json
"ControleFluxoCaixaConnectionString": "Server=localhost;Database=PDV_02;Uid=root;Pwd=q1w2e3r4;"
```

## Executando a Aplicação

1. Restaure as dependências: `dotnet restore`
2. Execute as migrações do banco de dados: `dotnet ef migrations add InitialMigration`
3. Aplique as migrações no banco de dados: `dotnet ef database update`
4. Execute o projeto: `dotnet run`






