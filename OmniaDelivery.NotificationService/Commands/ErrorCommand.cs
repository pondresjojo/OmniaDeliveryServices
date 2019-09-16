using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.NotificationService.Commands
{
    public class ErrorCommand: Command
    {

        public ErrorCommand()
        {
            EventDate = DateTime.UtcNow;
        }

        public string Id { get; set; }
        public string Code { get; set; }

        public DateTime EventDate { get; set; }

        public string Description { get; set; }

    }
}
