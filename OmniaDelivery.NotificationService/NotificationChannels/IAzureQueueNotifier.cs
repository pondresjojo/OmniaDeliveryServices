using OmniaDelivery.Infra.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.NotificationService.NotificationChannels
{
    public interface IAzureQueueNotifier
    {
        Task SendMessageAsync(string queuename, Command command);
    }
}
