using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using OmniaDelivery.Infra.Messaging;
using OmniaDelivery.NotificationService.NotificationChannels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.NotificationService.Implementations
{
    public class AzureQueueNotifier : IAzureQueueNotifier
    {

        private static IQueueClient queueClient;
        private string _endpoint;

        public AzureQueueNotifier(string endpoint)
        {

            _endpoint = endpoint;

        }

        public async Task SendMessageAsync(string queuename, Command command)
        {

            queueClient = new QueueClient(_endpoint, queuename, ReceiveMode.PeekLock);
            try
            {

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));
                var message = new Microsoft.Azure.ServiceBus.Message { Body = body };
                await queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }

            await Task.Delay(10);
        }
    }
}
