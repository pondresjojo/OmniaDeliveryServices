using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using OmniaDelivery.Infra.Implementations;
using OmniaDelivery.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniaDelivery.ConsumerService
{
    public class ConsumerManager : IHostedService, IMessageHandlerCallback
    {


        IMessageHandler _messageHandler;
        IMessagePublisher _messagePublisher;

        public ConsumerManager(IMessageHandler messageHandler, IMessagePublisher messagepublisher)
        {
            _messageHandler = messageHandler;
            _messagePublisher = messagepublisher;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }


        public async Task<bool> HandleMessageAsync(string messageType, string message, IDictionary<string, object> userproperties)
        {

            try
            {
                switch (messageType)
                {
                    case "Fulfilment":
                    default:
                        //hier aan de queue toevoegen
                        JObject order = JObject.Parse(message);
                        var id = userproperties.ContainsKey("Id") ? userproperties["Id"].ToString() : string.Empty;
                        var sendingdate = userproperties.ContainsKey("SendingDate") ? userproperties["SendingDate"].ToString() : string.Empty;
                        var dict = new Dictionary<string, object>() { { "Id", id }, { "OrderType", messageType }, { "SendingDate", sendingdate } };
                        await _messagePublisher.PublishMessageAsync(messageType, order, string.Empty, dict);
                        break;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;


        }


    }
}
