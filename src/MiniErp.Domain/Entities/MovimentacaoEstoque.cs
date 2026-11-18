using MiniErp.Domain.Enums;

namespace MiniErp.Domain.Entities
{
    public class MovimentacaoEstoque
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }
        public TipoMovimentacao Tipo { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataHora { get; set; }
        public string? Observacao { get; set; }
    }
}
