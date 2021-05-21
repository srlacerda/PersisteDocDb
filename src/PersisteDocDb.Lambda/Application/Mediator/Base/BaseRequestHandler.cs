
using MediatR;
using PersisteDocDb.Lambda.Domain.Base;
using PersisteDocDb.Lambda.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PersisteDocDb.Lambda.Application.Mediator.Base
{
    public abstract class BaseRequestHandler<TRequest> : IRequestHandler<TRequest, Result> where TRequest : IRequest<Result>
    {
        protected ILogger Logger;

        public BaseRequestHandler(ILogger logger)
        {
            Logger = logger;
        }

        internal abstract string ValidateRequest(TRequest request);

        protected abstract bool RequiresValidation();

        internal abstract Result Execute(TRequest request, CancellationToken cancellationToken);

        public Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
        {
            Result result = null;
            try
            {
                if (RequiresValidation())
                {
                    var validationMessage = ValidateRequest(request);

                    if (!string.IsNullOrEmpty(validationMessage))
                    {
                        //result = new Result
                        //{
                        //    Sucess = false,
                        //    Exception = new ArgumentException(validationMessage)
                        //};
                        //return Task.FromResult(result);
                        throw new ArgumentException(validationMessage);
                    };
                }

                result = Execute(request, cancellationToken);

                if (result == null)
                {
                    result = new Result() { Sucess = false, Exception = new Exception("The respective command execution returned a null response.") };
                }
                else
                {
                    result.Sucess = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "An error has occurred during the request.");
                throw;
                //var stringBuilder = new StringBuilder("An error has occurred during the request.");
                //stringBuilder.AppendLine($"Message: {e.Message}");
                //Logger.Error(stringBuilder.ToString());
                //result = new Result
                //{
                //    Exception = e,
                //    Sucess = false
                //};
                //return Task.FromResult(result);
            }

            return Task.FromResult(result);
        }
    }
}
