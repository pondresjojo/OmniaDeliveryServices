using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.TimeService.Events
{
    public class RealtimeTriggered : Event
    {
        public RealtimeTriggered(Guid messageId) : base(messageId)
        {
        }

        public string Folder { get; set; }
        public string Filter { get; set; }
        public string Customer { get; set; }
        public string Flow { get; set; }

    }
}
