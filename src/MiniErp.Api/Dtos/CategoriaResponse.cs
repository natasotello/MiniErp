
namespace MiniErp.Api.Dtos
{
    public record CategoriaResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativa { get; set; }
    }
}
