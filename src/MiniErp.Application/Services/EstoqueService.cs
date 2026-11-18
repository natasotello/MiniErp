using MiniErp.Domain.Entities;
using MiniErp.Domain.Enums;
using System.Diagnostics;

namespace MiniErp.Application.Services
{
    public class EstoqueService
    {
        public void ValidarQuantidade(int quantidade)
        {
            if (quantidade <= 0)
            {
                throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));
            }
        }

        public void ValidarEstoqueDisponivel(int estoqueAtual, int quantidade)
        {
            if(quantidade > estoqueAtual)
            {
                throw new InvalidOperationException("Quantidade solicitada maior que o estoque atual.");
            }
        }

        public void RegistrarSaida(Produto produto, int quantidade)
        {
            ValidarQuantidade(quantidade);

            ValidarEstoqueDisponivel(produto.EstoqueAtual, quantidade);

            produto.EstoqueAtual -= quantidade;
        }

        public void RegistrarEntrada(Produto produto, int quantidade)
        {
            ValidarQuantidade(quantidade);

            produto.EstoqueAtual += quantidade;
        }

        public void AplicarMovimentacao(Produto produto,TipoMovimentacao tipo,int quantidade)
        {
            switch(tipo)
            {
                case TipoMovimentacao.Entrada:
                    RegistrarEntrada(produto, quantidade);
                    break;
                case TipoMovimentacao.Saida:
                    RegistrarSaida(produto, quantidade);
                    break;
                default: throw new ArgumentException("Tipo de movimentação inválido.",nameof(tipo));
            }
        }

        public MovimentacaoEstoque CriarMovimentacao(Produto produto,TipoMovimentacao tipo,int quantidade,string? observacao)
        {
            AplicarMovimentacao(produto, tipo, quantidade);

            var movimentacao = new MovimentacaoEstoque
            {
                ProdutoId = produto.Id,
                Tipo = tipo,
                Quantidade = quantidade,
                DataHora = DateTime.UtcNow,
                Observacao = observacao
            };

            return movimentacao;
        }
    }
}