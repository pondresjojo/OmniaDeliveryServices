using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.WatchService.Events
{
    public class TriggerTriggered : Event
    {
        public TriggerTriggered(Guid messageId) : base(messageId)
        {
        }
    }
}
