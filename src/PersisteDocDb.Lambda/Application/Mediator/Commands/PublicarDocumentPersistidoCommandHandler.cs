using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pacote.Core.Domain.Model.Enums;
using PersisteDocDb.Lambda.Application.Mediator.Base;
using PersisteDocDb.Lambda.Domain.Base;
using PersisteDocDb.Lambda.Domain.Entities;
using PersisteDocDb.Lambda.Infrastructure.Logging;
using PersisteDocDb.Lambda.Infrastructure.Messaging;
using System;
using System.Text;
using System.Threading;

namespace PersisteDocDb.Lambda.Application.Mediator.Commands
{
    public class PublicarDocumentPersistidoCommandHandler : BaseRequestHandler<PublicarDocumentPersistidoCommand>
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonSqsClientHelper _amazonSqsClientHelper;
        public PublicarDocumentPersistidoCommandHandler(ILogger logger, IConfiguration configuration, IAmazonSqsClientHelper amazonSqsClientHelper)
            : base (logger)
        {
            _configuration = configuration;
            _amazonSqsClientHelper = amazonSqsClientHelper;
        }
        protected override bool RequiresValidation()
        {
            return true;
        }

        internal override Result Execute(PublicarDocumentPersistidoCommand request, CancellationToken cancellationToken)
        {
            Logger.Info($"PublicarDocumentPersistidoCommand. Message: '{request.Message}'");
            var documentPersistido = JsonConvert.DeserializeObject<DocumentPersistido>(request.Message);

            var queueName = _amazonSqsClientHelper.GetSqsPublicarDocumentPersistidoByMercado(documentPersistido.Mercado, _configuration);

            Logger.Info($"Sending Message to Queue - Queue: '{queueName}' - MessageBody: '{request.Message}'");
            var result = _amazonSqsClientHelper.SenMessageAsync(queueName, request.Message).Result;

            if (result.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Unable to sent message to queue '{queueName}'");
            }

            return new Result
            {
                Sucess = true
            };
        }

        internal override string ValidateRequest(PublicarDocumentPersistidoCommand request)
        {
            if (request.Message == null)
            {
                return "Request Cant Be Null";
            }

            var validationMessage = new StringBuilder();

            var documentPersistido = JsonConvert.DeserializeObject<DocumentPersistido>(request.Message);

            if (string.IsNullOrEmpty(documentPersistido.Id))
            {
                validationMessage.Append("Id is required ");
            }

            if (string.IsNullOrEmpty(documentPersistido.DocumentCollection))
            {
                validationMessage.Append("DocumentCollection is required ");
            }

            if (!Enum.IsDefined(typeof(MercadoEnum), documentPersistido.Mercado))
            {
                validationMessage.Append("Mercado does not exists ");
            }

            return validationMessage.ToString();
        }
    }
}
