using Amazon.SQS.Model;
using Moq.AutoMock;
using Pacote.Core.Domain.Model.Enums;
using PersisteDocDb.Lambda.Application.Mediator.Commands;
using PersisteDocDb.Lambda.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PersisteDocDb.Lambda.Tests
{
    [CollectionDefinition(nameof(DocumentPersistidoCollection))]

    public class DocumentPersistidoCollection : ICollectionFixture<DocumentPersistidoTestsFixture>
    { }
    public class DocumentPersistidoTestsFixture : IDisposable
    {
        public PublicarDocumentPersistidoCommandHandler PublicarDocumentPersistidoCommandHandler;
        public AutoMocker Mocker;

        public PublicarDocumentPersistidoCommandHandler ObterPublicarDocumentPersistidoCommandHandler()
        {
            Mocker = new AutoMocker();
            PublicarDocumentPersistidoCommandHandler = Mocker.CreateInstance<PublicarDocumentPersistidoCommandHandler>();
            return PublicarDocumentPersistidoCommandHandler;
        }

        public DocumentPersistido GerarDocumentPersistidoValido()
        {
            return new DocumentPersistido
            {
                DocumentCollection = "posicao",
                Id = "202",
                Mercado = MercadoEnum.DIGITAL_ASSETS
            };
        }

        public DocumentPersistido GerarDocumentPersistidoInvalido()
        {
            return new DocumentPersistido();
        }

        public string GerarQueueValida()
        {
            return "sqs-valida-posicoes-criptomoedas-datahub";
        }
        
        public Task<SendMessageResponse> GerarSqsSendMessageResponseOk()
        {
            var response = new SendMessageResponse();
            response.HttpStatusCode = HttpStatusCode.OK;
            return Task.FromResult(response);
        }

        public Task<SendMessageResponse> GerarSqsSendMessageResponseNotOk()
        {
            var response = new SendMessageResponse();
            response.HttpStatusCode = HttpStatusCode.InternalServerError;
            return Task.FromResult(response);
        }

        public void Dispose()
        {
        }
    }
}
