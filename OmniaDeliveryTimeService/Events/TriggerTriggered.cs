using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.TimeService.Events
{
    public class TriggerTriggered : Event
    {
        public TriggerTriggered(Guid messageId) : base(messageId)
        {
        }
    }
}
