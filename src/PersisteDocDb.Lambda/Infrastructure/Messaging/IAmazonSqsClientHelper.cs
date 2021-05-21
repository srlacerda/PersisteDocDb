using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using Pacote.Core.Domain.Model.Enums;
using System.Threading.Tasks;

namespace PersisteDocDb.Lambda.Infrastructure.Messaging
{
    public interface IAmazonSqsClientHelper
    {
        string GetSqsPublicarDocumentPersistidoByMercado(MercadoEnum mercado, IConfiguration configuration);
        Task<SendMessageResponse> SenMessageAsync(string queueName, string messageBody);
    }
}
