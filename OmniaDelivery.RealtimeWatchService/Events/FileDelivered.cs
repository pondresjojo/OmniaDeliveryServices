using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.WatchService.Events
{
    public class FileDelivered: Event
    {
        public FileDelivered(Guid messageId, Guid deliveryid, string filename, string flow, string customer) : base(messageId)
        {
            FileName = filename;
            Flow = flow;
            Customer = customer;
            DeliveryId = deliveryid;
        }
        public string FileName { get; set; }
        public string Flow { get; set; }
        public string Customer { get; set; }

        public Guid DeliveryId { get; set; }

    }
}
