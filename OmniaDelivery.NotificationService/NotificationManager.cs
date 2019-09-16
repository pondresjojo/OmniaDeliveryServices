using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using OmniaDelivery.Infra.Helpers;
using OmniaDelivery.Infra.Interfaces;
using OmniaDelivery.NotificationService.Commands;
using OmniaDelivery.NotificationService.Events;
using OmniaDelivery.NotificationService.NotificationChannels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniaDelivery.NotificationService
{
    public class NotificationManager : IHostedService, IMessageHandlerCallback
    {

        IMessageHandler _messageHandler;
        IAzureQueueNotifier _azurequeuenotifier;
        IEmailNotifier _emailnotifier;

        public NotificationManager(IMessageHandler messageHandler, IAzureQueueNotifier azurequeuenotifier, IEmailNotifier emailnotifier)
        {
            _messageHandler = messageHandler;
            _azurequeuenotifier = azurequeuenotifier;
            _emailnotifier = emailnotifier;
 
        }

        public async Task<bool> HandleMessageAsync(string messageType, string message, IDictionary<string, object> userproperties)
        {
            try
            {
                JObject messageObject = MessageSerializer.Deserialize(message);
                switch (messageType)
                {
                    case "DeliveryCompleted":
                        await HandleAsync(messageObject.ToObject<DeliveryCompleted>());
                        break;
                    case "DeliveryFaulted":
                        await HandleAsync(messageObject.ToObject<DeliveryFaulted>());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error while handling {messageType} event.");
            }

            return true;
        }


        private async Task HandleAsync(DeliveryCompleted dr)
        {

            await _azurequeuenotifier.SendMessageAsync("status", new StatusCommand());

            //Customer customer = new Customer
            //{
            //    CustomerId = cr.CustomerId,
            //    Name = cr.Name,
            //    TelephoneNumber = cr.TelephoneNumber,
            //    EmailAddress = cr.EmailAddress
            //};

            //Log.Information("Register customer: {Id}, {Name}, {TelephoneNumber}, {Email}",
            //    customer.CustomerId, customer.Name, customer.TelephoneNumber, customer.EmailAddress);

            //await _repo.RegisterCustomerAsync(customer);
            //await Task.Delay(1000);

        }


        private async Task HandleAsync(DeliveryFaulted dr)
        {
            //Customer customer = new Customer
            //{
            //    CustomerId = cr.CustomerId,
            //    Name = cr.Name,
            //    TelephoneNumber = cr.TelephoneNumber,
            //    EmailAddress = cr.EmailAddress
            //};

            //Log.Information("Register customer: {Id}, {Name}, {TelephoneNumber}, {Email}",
            //    customer.CustomerId, customer.Name, customer.TelephoneNumber, customer.EmailAddress);

            //await _repo.RegisterCustomerAsync(customer);
            await Task.Delay(1000);

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
