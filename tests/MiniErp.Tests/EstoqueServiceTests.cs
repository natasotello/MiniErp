using Xunit;
using MiniErp.Application.Services;
using MiniErp.Domain.Entities;
using MiniErp.Domain.Enums;

namespace MiniErp.Tests;

public class EstoqueServiceTests
{
    private readonly EstoqueService _service = new();

    // TESTE 1 - Entrada deve aumentar o estoque
    [Fact]
    public void Entrada_DeveAumentarEstoque()
    {
        var produto = new Produto { EstoqueAtual = 15 };

        _service.RegistrarEntrada(produto, 5);

        Assert.Equal(20, produto.EstoqueAtual);
    }

    // TESTE 2 - Saida deve diminuir o estoque
    [Fact]
    public void Saida_DeveDiminuirEstoque()
    {
        var produto = new Produto { EstoqueAtual = 15 };

        _service.RegistrarSaida(produto, 5);

        Assert.Equal(10, produto.EstoqueAtual);
    }

    // TESTES 3 E 4 - Quantidades zero e negativa
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void QuantidadeInvalida_DeveLancarExcecao(int quantidade)
    {
        Assert.Throws<ArgumentException>(
            () => _service.ValidarQuantidade(quantidade));
    }

    // TESTE 5 - Impedir saida com estoque insuficiente
    [Fact]
    public void SaidaSemEstoque_DeveSerImpedida()
    {
        var produto = new Produto { EstoqueAtual = 15 };

        Assert.Throws<InvalidOperationException>(
            () => _service.RegistrarSaida(produto, 20));

        Assert.Equal(15, produto.EstoqueAtual);
    }

    // TESTE 6 - Permitir que o estoque fique zerado
    [Fact]
    public void SaidaIgualAoEstoque_DeveZerarSaldo()
    {
        var produto = new Produto { EstoqueAtual = 15 };

        _service.RegistrarSaida(produto, 15);

        Assert.Equal(0, produto.EstoqueAtual);
    }

    // TESTE 7 - Criar movimentacao com dados corretos
    [Fact]
    public void CriarMovimentacao_DeveRegistrarDadosCorretos()
    {
        var produto = new Produto
        {
            Id = 8,
            EstoqueAtual = 15
        };

        var movimentacao = _service.CriarMovimentacao(
            produto,
            TipoMovimentacao.Saida,
            5,
            "Teste automatizado");

        Assert.Equal(8, movimentacao.ProdutoId);
        Assert.Equal(TipoMovimentacao.Saida, movimentacao.Tipo);
        Assert.Equal(5, movimentacao.Quantidade);
        Assert.Equal(10, produto.EstoqueAtual);
        Assert.Equal("Teste automatizado", movimentacao.Observacao);
    }
}
