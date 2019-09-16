﻿using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.TimeService.Events
{
    public class RealtimeWatcherAdded: Event
    {

        public string Flow { get; set; }
        public string Customer { get; set; }
        public string Folder { get; set; }
        public string Filter { get; set; }


    }
}
