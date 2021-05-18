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
    public class PersistePosicaoCommandHandler : BaseRequestHandler<PersistePosicaoCommand>
    {
        private readonly IMediator _mediator;
        private readonly IDocumentRepository<PosicaoDocument> _documentRepository;
        private readonly IDocumentFactory<PosicaoDocument> _documentFactory;
        public PersistePosicaoCommandHandler(IMediator mediator, ILogger logger, 
            IDocumentRepository<PosicaoDocument> documentRepository, IDocumentFactory<PosicaoDocument> documentFactory)
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

        internal override Result Execute(PersistePosicaoCommand request, CancellationToken cancellationToken)
        {
            Logger.Info($"PersistePosicaoCommand. Message: '{request.Message}'");
            var document = _documentFactory.DeserializeDocument(request.Message);

            Logger.Info($"Persisting at DocumentDB - Id: '{document.Id}'");
            var replaceResult = _documentRepository.PersisteDocumentReplaceOne(document);
            Logger.Info(replaceResult.Equals(0) ? "Document inserted" : "Document updated");

            var createPublicarDocumentPersistidoCommand = _documentFactory.CreatePublicarDocumentPersistidoCommand(document);

            var result = _mediator.Send(createPublicarDocumentPersistidoCommand, cancellationToken).Result;
            
            return new Result
            {
                Sucess = true
            };
        }

        internal override string ValidateRequest(PersistePosicaoCommand request)
        {
            if (request.Message == null)
            {
                return "Request Cant Be Null";
            }

            var validationMessage = new StringBuilder();

            var document = _documentFactory.DeserializeDocument(request.Message);

            if (document.DataPosicao.Equals(DateTime.MinValue))
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
