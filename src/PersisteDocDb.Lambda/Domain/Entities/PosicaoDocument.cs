using Pacote.Core.Domain.Model.Contratos;
using Pacote.Core.Domain.Util.DocumentDB;

namespace PersisteDocDb.Lambda.Domain.Entities
{
    public class PosicaoDocument : Posicao, IDocument
    {
        public string Id => $"{DataPosicao:yyyyMMdd}_{CodigoFatura}_{CodigoSistema}";
    }
}
