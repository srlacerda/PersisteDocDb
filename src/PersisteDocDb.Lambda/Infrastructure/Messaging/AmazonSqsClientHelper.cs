using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using Pacote.Core.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PersisteDocDb.Lambda.Infrastructure.Messaging
{
    public class AmazonSqsClientHelper : IAmazonSqsClientHelper
    {
        private readonly IAmazonSQS _amazonSQS;
        public AmazonSqsClientHelper(IAmazonSQS amazonSQS)
        {
            _amazonSQS = amazonSQS;
        }
        public string GetSqsPublicarDocumentPersistido(MercadoEnum mercado, IConfiguration configuration)
        {
            string settings = $"SqsValidaPosicaoByMercado";
            string mercadoString = mercado.ToString();
            return configuration[$"{settings}:{mercadoString}"];
        }

        public async Task<SendMessageResponse> SenMessageAsync(string queueName, string messageBody)
        {
            GetQueueUrlResponse getQueueUrlResponse = await _amazonSQS.GetQueueUrlAsync(queueName);

            var request = new SendMessageRequest(
                queueUrl: getQueueUrlResponse.QueueUrl,
                messageBody: messageBody);

            var result = await _amazonSQS.SendMessageAsync(request, new CancellationToken());

            return result;
        }
    }
}
