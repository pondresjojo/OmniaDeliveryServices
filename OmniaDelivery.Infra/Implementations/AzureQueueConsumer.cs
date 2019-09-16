using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json.Linq;
using OmniaDelivery.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.Infra.Implementations
{
    public class AzureQueueConsumer: IMessageHandler
    {

        private readonly string _endpoint;
        private readonly string _queuename;
        private IQueueClient _queueClient;


        private IMessageHandlerCallback _callback;


        public AzureQueueConsumer(string endpoint, string queuename)
        {
            _endpoint = endpoint;
            _queuename = queuename;
        }

        public void Start(IMessageHandlerCallback callback)
        {
            _callback = callback;

            _queueClient = new QueueClient(_endpoint, _queuename, ReceiveMode.PeekLock);

            try
            {
                // Register a OnMessage callback 
                _queueClient.RegisterMessageHandler(
                    async (message, token) =>
                    {
                        if (await HandleEvent(message))
                        {
                            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
                        }
                    },
                    new MessageHandlerOptions(exceptionReceivedEventArgs =>
                    {
                        Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
                        return Task.CompletedTask;
                    })
                    { MaxConcurrentCalls = 1, AutoComplete = false });
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }

        }

        public void Stop()
        {
            _queueClient.CloseAsync();
        }


        private Task<bool> HandleEvent(Message message)
        {
            // determine messagetype
            var ordertype = message.UserProperties.ContainsKey("OrderType") ? message.UserProperties["OrderType"].ToString() : string.Empty;

            var jsonstring = Encoding.UTF8.GetString(message.Body);

            return _callback.HandleMessageAsync(ordertype, jsonstring, message.UserProperties);

        }

    }
}
