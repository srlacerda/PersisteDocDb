using MediatR;
using PersisteDocDb.Lambda.Domain.Exceptions;
using PersisteDocDb.Lambda.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PersisteDocDb.Lambda.Application.Mediator.Base
{
    public abstract class BaseNotificationHandler<TNotification> : INotificationHandler<TNotification> where TNotification : INotification
    {
        protected ILogger Logger;

        protected BaseNotificationHandler(ILogger logger)
        {
            Logger = logger;
        }

        internal abstract string ValidateRequest(TNotification notification);

        protected abstract bool RequiresValidation();

        internal abstract void Execute(TNotification request, CancellationToken cancellationToken);
        public Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            try
            {
                if (RequiresValidation())
                {
                    var validationMessage = ValidateRequest(notification);

                    if (!string.IsNullOrEmpty(validationMessage))
                    {
                        return Task.FromException(new InvalidNotificationException(typeof(TNotification), validationMessage));
                    }
                }

            }
            catch (Exception e)
            {
                var stringBuilder = new StringBuilder("An error has occurred during the request.");

                //stringBuilder.AppendLine($"Message: {e.GetFullMessage()}");
                stringBuilder.AppendLine($"Message: {e.Message}");

                Logger.Error(stringBuilder.ToString());

                return Task.FromException(e);
            }

            return Task.CompletedTask;
        }
    }
}
