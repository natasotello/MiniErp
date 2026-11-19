# Mini ERP — API de Controle de Estoque

API REST desenvolvida em **C# e ASP.NET Core (.NET 10)** para cadastrar produtos, categorias e fornecedores, controlar entradas e saídas de estoque e consultar o histórico de movimentações.

Projeto de estudo e portfólio, com foco em organização em camadas, **Entity Framework Core**, **SQL Server**, regras de negócio e testes automatizados.

## Funcionalidades

- Cadastro, listagem, consulta, atualização e exclusão de produtos.
- Cadastro e consulta de categorias e fornecedores.
- Associação opcional de produtos a categorias e fornecedores.
- Restrição de CNPJ duplicado no cadastro de fornecedores; índices únicos para CNPJ e SKU no banco.
- Registro de **entradas e saídas** com data, quantidade e observação.
- Validação de quantidade positiva e bloqueio de saídas com estoque insuficiente.
- Atualização do saldo do produto e gravação do histórico na mesma operação de persistência.
- Consulta de movimentações por produto, da mais recente para a mais antiga.
- Três relatórios SQL: estoque atual, estoque baixo e histórico de movimentações.
- Testes unitários das principais regras de estoque com xUnit.

**Regra de estoque:** produtos novos começam com saldo zero. O saldo não é alterado diretamente pelos endpoints de criação ou atualização de produtos; entradas e saídas devem passar pelo endpoint de movimentações.

## Tecnologias

- C# / .NET 10
- ASP.NET Core Web API (controllers)
- Entity Framework Core 10 e SQL Server
- Swagger / Swashbuckle
- xUnit
- Git / GitHub

## Estrutura do projeto

```text
MiniErp/
├── src/
│   ├── MiniErp.Api/             # Endpoints HTTP, DTOs e configuração
│   ├── MiniErp.Application/     # Regras de negócio do EstoqueService
│   ├── MiniErp.Domain/          # Entidades e enum de movimentação
│   └── MiniErp.Infrastructure/  # AppDbContext, mapeamentos e migrations
├── tests/
│   └── MiniErp.Tests/          # Testes unitários com xUnit
├── sql/
│   └── relatorios.sql          # Consultas para o SQL Server
└── MiniErp.slnx
```

## Endpoints principais

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/Produtos` | Lista produtos |
| GET | `/api/Produtos/{id}` | Consulta produto por ID |
| POST | `/api/Produtos` | Cadastra produto com estoque inicial zero |
| PUT | `/api/Produtos/{id}` | Atualiza dados do produto, exceto estoque |
| DELETE | `/api/Produtos/{id}` | Exclui produto (sujeito às restrições de relacionamento) |
| GET | `/api/Categorias` | Lista categorias |
| GET | `/api/Categorias/{id}` | Consulta categoria |
| POST | `/api/Categorias` | Cadastra categoria |
| GET | `/api/Fornecedores` | Lista fornecedores |
| GET | `/api/Fornecedores/{id}` | Consulta fornecedor |
| POST | `/api/Fornecedores` | Cadastra fornecedor |
| POST | `/api/MovimentacoesEstoque` | Registra entrada ou saída |
| GET | `/api/MovimentacoesEstoque/produto/{produtoId}` | Consulta histórico de um produto |

### Exemplo de movimentação

Requisição `POST /api/MovimentacoesEstoque`:

```json
{
  "produtoId": 1,
  "tipo": 1,
  "quantidade": 5,
  "observacao": "Entrada de mercadoria"
}
```

O `produtoId` deve identificar um produto existente. Para o campo `tipo`, use **1 = Entrada** ou **2 = Saída**. Uma movimentação válida retorna **201 Created**. Produto inexistente retorna **404**, quantidade ou tipo inválido retorna **400**, e estoque insuficiente retorna **409**.

## Como executar localmente

**Pré-requisitos:** SQL Server, .NET com suporte à execução de projetos `net10.0`, e a ferramenta `dotnet-ef` para aplicar migrations.

1. Clone o repositório:

   ```bash
   git clone https://github.com/natasotello/MiniErp.git
   cd MiniErp
   ```

2. Configure a string de conexão `DefaultConnection` em `src/MiniErp.Api/appsettings.json` de acordo com sua instância local do SQL Server. O projeto inclui uma configuração de exemplo com autenticação integrada do Windows.

3. Aplique as migrations:

   ```bash
   dotnet ef database update --project src/MiniErp.Infrastructure --startup-project src/MiniErp.Api
   ```

4. Execute a API em ambiente de desenvolvimento:

   ```bash
   dotnet run --project src/MiniErp.Api --launch-profile http
   ```

5. Abra o Swagger em **http://localhost:5226/swagger**. Essa URL corresponde ao perfil HTTP presente em `launchSettings.json`.

## Testes automatizados

Na raiz do projeto, execute:

```bash
dotnet test
```

Os testes de `EstoqueServiceTests.cs` verificam entradas, saídas, quantidades inválidas, estoque insuficiente, saldo zero e dados do histórico. Os testes são unitários e **não substituem testes de integração** com a API e o banco de dados.

## Relatórios SQL

Abra `sql/relatorios.sql` no SQL Server Management Studio, selecione o banco de dados do projeto e execute as consultas. O relatório de estoque baixo usa um limite configurável pela variável `@EstoqueMinimo`.

## Escopo e limitações

Este é um **projeto de portfólio**, não um ERP pronto para produção. A versão atual não implementa autenticação/autorização, pedidos, compras, validação completa de CNPJ ou controle de concorrência para movimentações simultâneas. As validações de entrada e o tratamento de erros de integridade do banco ainda podem ser ampliados.

O objetivo é demonstrar fundamentos de APIs REST, modelagem relacional, EF Core, SQL e separação de regras de negócio.
