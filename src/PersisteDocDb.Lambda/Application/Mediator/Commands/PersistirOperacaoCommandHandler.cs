using MediatR;
using Pacote.Core.Domain.Model.Enums;
using PersisteDocDb.Lambda.Application.Mediator.Base;
using PersisteDocDb.Lambda.Domain.Base;
using PersisteDocDb.Lambda.Domain.Entities;
using PersisteDocDb.Lambda.Infrastructure.Factory;
using PersisteDocDb.Lambda.Infrastructure.Logging;
using PersisteDocDb.Lambda.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PersisteDocDb.Lambda.Application.Mediator.Commands
{
    public class PersistirOperacaoCommandHandler : BaseRequestHandler<PersistirOperacaoCommand>
    {
        
        private readonly IMediator _mediator;
        private readonly IDocumentRepository<OperacaoDocument> _documentRepository;
        private readonly IDocumentFactory<OperacaoDocument> _documentFactory;
        public PersistirOperacaoCommandHandler(IMediator mediator, ILogger logger,
            IDocumentRepository<OperacaoDocument> documentRepository, IDocumentFactory<OperacaoDocument> documentFactory)
            : base(logger)
        {
            _mediator = mediator;
            _documentFactory = documentFactory;
            _documentRepository = documentRepository;
        }
        protected override bool RequiresValidation()
        {
            return true;
        }

        internal override Result Execute(PersistirOperacaoCommand request, CancellationToken cancellationToken)
        {
            Logger.Info($"PersisteOperacaoCommand. Message: '{request.Message}'");
            var document = _documentFactory.DeserializeDocument(request.Message);

            Logger.Info($"Persisting at DocumentDB - Id: '{document.Id}'");
            var replaceResult = _documentRepository.PersistirDocumentReplaceOne(document);
            Logger.Info(replaceResult.Equals(0) ? "Document inserted" : "Document updated");

            var createPublicarDocumentPersistidoCommand = _documentFactory.CreatePublicarDocumentPersistidoCommand(document);

            var result = _mediator.Send(createPublicarDocumentPersistidoCommand, cancellationToken).Result;

            return new Result
            {
                Sucess = true
            };
        }

        internal override string ValidateRequest(PersistirOperacaoCommand request)
        {
            if (request.Message == null)
            {
                return "Request Cant Be Null";
            }

            var validationMessage = new StringBuilder();

            var document = _documentFactory.DeserializeDocument(request.Message);

            if (document.DataOperacao.Equals(DateTime.MinValue))
            {
                validationMessage.Append("DataPosicao is required ");
            }

            if (string.IsNullOrEmpty(document.CodigoFatura))
            {
                validationMessage.Append("CodigoFatura is required ");
            }

            if (!Enum.IsDefined(typeof(MercadoEnum), document.Mercado))
            {
                validationMessage.Append("Mercado does not exists ");
            }

            return validationMessage.ToString();
        }
    }
}
