using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.InputProcessingService.Model
{
    public class FileDelivered: Event
    {



        public string FileName { get; set; }
        public string Flow { get; set; }
        public string Customer { get; set; }

        public Guid DeliveryId { get; set; }


    }
}
