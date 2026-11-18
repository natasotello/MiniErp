
namespace MiniErp.Api.Dtos
{
    public record CriarProdutoRequest
    {
        public string Sku { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoVenda { get; set; }
        public int EstoqueAtual { get; set; }
        public bool Ativo { get; set; }
        public int? CategoriaId { get; set; }
        public int? FornecedorId { get; set; }
    }
}
