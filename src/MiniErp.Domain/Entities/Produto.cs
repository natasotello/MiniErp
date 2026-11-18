
namespace MiniErp.Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoVenda { get; set; }
        public int EstoqueAtual { get; set; }
        public bool Ativo { get; set; }
        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        public int? FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
    }
}
