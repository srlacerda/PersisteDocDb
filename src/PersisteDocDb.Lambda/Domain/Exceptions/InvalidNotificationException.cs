using System;
using System.Collections.Generic;
using System.Text;

namespace PersisteDocDb.Lambda.Domain.Exceptions
{
    public class InvalidNotificationException : Exception
    {
        public Type NotificationType {get; set; }
        public InvalidNotificationException(Type notificationType, string description) : base(description)
        {
            this.NotificationType = notificationType;
        }
    }
}
