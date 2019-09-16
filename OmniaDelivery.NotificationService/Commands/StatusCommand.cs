using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.NotificationService.Commands
{
    class StatusCommand: Command
    {
        public StatusCommand()
        {
            EventDate = DateTime.UtcNow;
        }
        public string Id { get; set; }
        public int Status { get; set; }

        public DateTime EventDate { get; set; }
    }
}
