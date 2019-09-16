using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.InputProcessingService.Model
{
    public class BatchDelivered: Event
    {
        public string Folder { get; set; }
        public string Flow { get; set; }
        public string Customer { get; set; }

        public int NumberOfFiles { get; set; }



    }
}
