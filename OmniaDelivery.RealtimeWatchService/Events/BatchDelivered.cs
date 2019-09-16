using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.WatchService.Events
{
    public class BatchDelivered: Event
    {

        public BatchDelivered(Guid messageId) : base(messageId)
        {
        }

        public string Folder { get; set; }
        public string Flow { get; set; }
        public string Customer { get; set; }

        public int NumberOfFiles { get; set; }



    }
}
