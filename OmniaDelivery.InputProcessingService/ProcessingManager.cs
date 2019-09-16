using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using OmniaDelivery.Infra.Helpers;
using OmniaDelivery.Infra.Interfaces;
using OmniaDelivery.InputProcessingService.Model;
using OmniaDelivery.InputProcessingService.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniaDelivery.InputProcessingService
{
    public class ProcessingManager : IHostedService, IMessageHandlerCallback
    {

        IMessageHandler _messageHandler;
        IMessagePublisher _messagePublisher;
        ISingleService _service;

        public ProcessingManager(IMessageHandler messageHandler, IMessagePublisher messagepublisher, ISingleService singleservice)
        {
            _messageHandler = messageHandler;
            _messagePublisher = messagepublisher;
            _service = singleservice;
        }

        public async Task<bool> HandleMessageAsync(string messageType, string message, IDictionary<string, object> userproperties)
        {

            try
            {
                JObject messageObject = MessageSerializer.Deserialize(message);
                switch (messageType)
                {
                    case "FileDelivered":
                        await HandleAsync(messageObject.ToObject<FileDelivered>());
                        break;
                    case "BatchDelivered":
                        await HandleAsync(messageObject.ToObject<BatchDelivered>());
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


        private async Task HandleAsync(BatchDelivered batchDelivered)
        {





            await Task.Delay(2000);

        }


        private async Task HandleAsync(FileDelivered fileDelivered)
        {

            DeliveryCompleted e = await _service.ProcessSingleFile(fileDelivered);

            //await _messagePublisher.PublishMessageAsync(e.MessageType, e, "DataService", null);
            //await _messagePublisher.PublishMessageAsync(e.MessageType, e, "notification", null);

            //var jobs = await _repo.GetMaintenanceJobsToBeInvoicedAsync();
            //foreach (var jobsPerCustomer in jobs.GroupBy(job => job.CustomerId))
            //{
            //    DateTime invoiceDate = DateTime.Now;
            //    string customerId = jobsPerCustomer.Key;
            //    Customer customer = await _repo.GetCustomerAsync(customerId);
            //    Invoice invoice = new Invoice
            //    {
            //        InvoiceId = $"{invoiceDate.ToString("yyyyMMddhhmmss")}-{customerId.Substring(0, 4)}",
            //        InvoiceDate = invoiceDate.Date,
            //        CustomerId = customer.CustomerId,
            //        JobIds = string.Join('|', jobsPerCustomer.Select(j => j.JobId))
            //    };

            //    StringBuilder specification = new StringBuilder();
            //    decimal totalAmount = 0;
            //    foreach (var job in jobsPerCustomer)
            //    {
            //        TimeSpan duration = job.EndTime.Value.Subtract(job.StartTime.Value);
            //        decimal amount = Math.Round((decimal)duration.TotalHours * HOURLY_RATE, 2);
            //        totalAmount += amount;
            //        specification.AppendLine($"{job.EndTime.Value.ToString("dd-MM-yyyy")} : {job.Description} on vehicle with license {job.LicenseNumber} - Duration: {duration.TotalHours} hour - Amount: &euro; {amount}");
            //    }
            //    invoice.Specification = specification.ToString();
            //    invoice.Amount = totalAmount;

            //    await SendInvoice(customer, invoice);
            //    await _repo.RegisterInvoiceAsync(invoice);

            //    Log.Information("Invoice {Id} sent to {Customer}", invoice.InvoiceId, customer.Name);
            //}
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
