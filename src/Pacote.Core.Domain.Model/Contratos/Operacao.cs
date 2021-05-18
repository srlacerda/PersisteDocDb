using Pacote.Core.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacote.Core.Domain.Model.Contratos
{
    public class Operacao
    {
        public MercadoEnum Mercado { get; set; }
        public string CodigoFatura { get; set; }
        public string CodigoFaturaOrigem { get; set; }
        public DateTime DataOperacao { get; set; }
        public int CodigoSistema { get; set; }
        public string Ativo { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
