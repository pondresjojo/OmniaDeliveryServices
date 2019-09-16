using Microsoft.Extensions.Hosting;
using OmniaDelivery.Infra.Interfaces;
using OmniaDelivery.ValidationService.Models;
using OmniaDelivery.ValidationService.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniaDelivery.ValidationService
{
    public class ValidationManager : IHostedService, IMessageHandlerCallback
    {

        IMessageHandler _messageHandler;
        IMessagePublisher _messagePublisher;
        IDeliveryService _service;

        public ValidationManager(IMessageHandler messageHandler, IMessagePublisher messagepublisher, IDeliveryService deliveryservice)
        {
            _messageHandler = messageHandler;
            _messagePublisher = messagepublisher;
            _service = deliveryservice;
        }
        public async Task<bool> HandleMessageAsync(string messageType, string message, IDictionary<string, object> userproperties)
        {


            try
            {
                var id = Encoding.UTF8.GetString((byte[])userproperties["Id"]);
                var ordertype = Encoding.UTF8.GetString((byte[])userproperties["OrderType"]);
                var sendingdate = Encoding.UTF8.GetString((byte[])userproperties["SendingDate"]);

                List<ValueDTO> values = new List<ValueDTO>();
                values.Add(new ValueDTO() { Key = "Id", Value = id });
                values.Add(new ValueDTO() { Key = "OrderType", Value = ordertype });
                values.Add(new ValueDTO() { Key = "SendingDate", Value = sendingdate });


                var valresponse = await _service.Validate("CardCompos", ".json", message);

                var streamid = Guid.NewGuid();
                var filename = string.Format("{0}_{1}.json", id, messageType);
                if(await _service.AddToStorage(streamid, message, filename, "json", valresponse.Valid))
                {
                    Guid headerid = Guid.NewGuid();
                    var xx = await _service.AddHeader(headerid, "CardCompos");
                    var yy = await _service.AddFileExtension(headerid, streamid, filename, valresponse.Valid, string.Join("\n", valresponse.Messages), values);

                    DeliveryCompleted e = await _service.CreateFowardingCommand(headerid);
                    await _messagePublisher.PublishMessageAsync(messageType,e, "DataService", null);
                    //await _messagePublisher.PublishMessageAsync(messageType, e, "", null);
                    await _messagePublisher.PublishMessageAsync(messageType, e, "notification", null);
                }


                //scaler aanroepen

                //status terugkoppelen.


            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
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
    }
}
