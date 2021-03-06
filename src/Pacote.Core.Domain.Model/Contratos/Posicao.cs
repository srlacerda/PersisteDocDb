using Pacote.Core.Domain.Model.Enums;
using System;

namespace Pacote.Core.Domain.Model.Contratos
{
    public class Posicao
    {
        public MercadoEnum Mercado { get; set; }
        public string CodigoFatura { get; set; }
        public DateTime DataPosicao { get; set; }
        public int CodigoSistema { get; set; }
        public string Ativo { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
