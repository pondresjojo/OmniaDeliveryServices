using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.InputProcessingService.Model
{
    public class DeliveryCompleted : Event
    {

        public readonly string UUID;
        public readonly string CustomerNumber;
        public readonly string CustomerFlow;
        public readonly string NextService;


        public DeliveryCompleted(Guid messageId, string UUID, string CustomerNumber, string CustomerFlow, string NextService) : base(messageId)
        {
            this.UUID = UUID;
            this.CustomerNumber = CustomerNumber;
            this.CustomerFlow = CustomerFlow;
            this.NextService = NextService;
        }
    }
}
