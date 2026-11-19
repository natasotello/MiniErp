-- =========================================
-- RELATÓRIO 1 - ESTOQUE ATUAL
-- =========================================

SELECT
    p.Id,
    p.Sku,
    p.Nome AS Produto,
    c.Nome AS Categoria,
    f.RazaoSocial AS Fornecedor,
    p.EstoqueAtual,
    p.PrecoVenda,
    p.EstoqueAtual * p.PrecoVenda AS ValorEmEstoque
FROM Produtos p
LEFT JOIN Categorias c
    ON p.CategoriaId = c.Id
LEFT JOIN Fornecedores f
    ON p.FornecedorId = f.Id
ORDER BY p.Nome;

GO

-- =========================================
-- RELATÓRIO 2 - PRODUTOS COM ESTOQUE BAIXO
-- =========================================

DECLARE @EstoqueMinimo INT = 20;

SELECT
    Id,
    Sku,
    Nome AS Produto,
    EstoqueAtual,
    CASE
        WHEN EstoqueAtual = 0 THEN 'Sem estoque'
        ELSE 'Estoque baixo'
    END AS Situacao
FROM Produtos
WHERE EstoqueAtual <= @EstoqueMinimo
ORDER BY EstoqueAtual ASC;

GO

-- =========================================
-- RELATÓRIO 3 - HISTÓRICO DE MOVIMENTAÇÕES
-- =========================================

SELECT
    m.Id AS MovimentacaoId,
    p.Nome AS Produto,
    CASE
        WHEN m.Tipo = 1 THEN 'Entrada'
        WHEN m.Tipo = 2 THEN 'Saída'
        ELSE 'Desconhecido'
    END AS TipoMovimentacao,
    m.Quantidade,
    m.DataHora,
    m.Observacao
FROM MovimentacoesEstoque m
INNER JOIN Produtos p
    ON m.ProdutoId = p.Id
ORDER BY m.DataHora DESC;

GO