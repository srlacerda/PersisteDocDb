using Pacote.Core.Domain.Model.Contratos;
using Pacote.Core.Domain.Util.DocumentDB;

namespace PersisteDocDb.Lambda.Domain.Entities
{
    public class OperacaoDocument : Operacao, IDocument
    {
        public string Id => $"{DataOperacao:yyyyMMdd}_{CodigoFatura}_{CodigoFaturaOrigem}_{CodigoSistema}";
    }
}
