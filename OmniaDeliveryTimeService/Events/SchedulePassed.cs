using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.TimeService.Events
{
    class SchedulePassed : Event
    {
        public SchedulePassed(Guid messageId) : base(messageId)
        {

        }
    }
}
