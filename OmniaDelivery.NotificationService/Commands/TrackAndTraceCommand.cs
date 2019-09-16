using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.NotificationService.Commands
{
    public class TrackAndTraceCommand: Command
    {

        public TrackAndTraceCommand()
        {
            EventDate = DateTime.UtcNow;
        }
        public string Id { get; set; }
        public string[] TrackTraceCode { get; set; }
        public string CarrierCode { get; set; }
        public string TcsCode { get; set; }

        public DateTime EventDate { get; set; }
    }
}
