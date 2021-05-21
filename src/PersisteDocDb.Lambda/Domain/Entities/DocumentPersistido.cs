using Pacote.Core.Domain.Model.Enums;

namespace PersisteDocDb.Lambda.Domain.Entities
{
    public class DocumentPersistido
    {
        public string DocumentCollection { get; set; }
        public string Id { get; set; }

        public MercadoEnum Mercado { get; set; }
    }
}
