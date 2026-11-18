
namespace MiniErp.Api.Dtos
{
    public record ProdutoResponse
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoVenda { get; set; }
        public int EstoqueAtual { get; set; }
        public bool Ativo { get; set; }
        public int? CategoriaId { get; set; }
        public int? FornecedorId { get; set; }
    }
}
