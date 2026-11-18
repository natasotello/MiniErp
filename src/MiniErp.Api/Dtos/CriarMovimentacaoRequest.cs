using System;
using System.Collections.Generic;
using System.Text;
using MiniErp.Domain.Enums;

namespace MiniErp.Api.Dtos
{
    public record CriarMovimentacaoRequest
    {
        public int ProdutoId { get; set; }
        public TipoMovimentacao Tipo { get; set; }
        public int Quantidade { get; set; }
        public string? Observacao { get; set; }
    }
}
