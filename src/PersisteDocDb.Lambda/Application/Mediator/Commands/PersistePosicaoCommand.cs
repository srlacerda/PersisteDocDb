using MediatR;
using PersisteDocDb.Lambda.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersisteDocDb.Lambda.Application.Mediator.Commands
{
    public class PersistePosicaoCommand : IRequest<Result>
    {
        public string Message { get; set; }
    }
}
