﻿using MediatR;
using PersisteDocDb.Lambda.Domain.Base;

namespace PersisteDocDb.Lambda.Application.Mediator.Commands
{
    public class PersisteOperacaoCommand : IRequest<Result>
    {
        public string Message { get; set; }
    }
}
