
namespace MiniErp.Api.Dtos
{
    public record CriarCategoriaRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativa { get; set; }
    }
}
