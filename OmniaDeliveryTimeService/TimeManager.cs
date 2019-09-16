using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using OmniaDelivery.Infra.Helpers;
using OmniaDelivery.Infra.Interfaces;
using OmniaDelivery.TimeService.Events;
using OmniaDelivery.TimeService.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OmniaDelivery.TimeService
{
    public class TimeManager : IHostedService, IMessageHandlerCallback
    {
        DateTime _lastCheck;
        CancellationTokenSource _cancellationTokenSource;
        IMessagePublisher _messagePublisher;
        IMessageHandler _messageHandler;
        ITimerService _realtimeservice;


        public TimeManager(IMessagePublisher messagePublisher, ITimerService rtservice)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _lastCheck = DateTime.Now;
            _messagePublisher = messagePublisher;
            _realtimeservice = rtservice;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(RealTimeJobs(), TriggerJobs(), SchedulesJobs());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        private async Task RealTimeJobs()
        {

            //initialiseren realtimejobs

            await _realtimeservice.InitAsync();
            


            while (true)
            {

                var triggers = await _realtimeservice.Process();
                //if (DateTime.Now.Subtract(_lastCheck).Days > 0)
                //{

                foreach(var tr in triggers)
                {
                    await _messagePublisher.PublishMessageAsync(tr.MessageType, tr, "");
                }


                //}
                await Task.Delay(60000);
            }
        }

        private async Task TriggerJobs()
        {
            while (true)
            {
                if (DateTime.Now.Subtract(_lastCheck).Days > 0)
                {
                    Log.Information($"Day has passed!");
                    _lastCheck = DateTime.Now;
                    DateTime passedDay = _lastCheck.AddDays(-1);
                    TriggerTriggered e = new TriggerTriggered(Guid.NewGuid());
                    //await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
                }
                await Task.Delay(10000);
            }
        }


        private async Task SchedulesJobs()
        {
            while (true)
            {
                if (DateTime.Now.Subtract(_lastCheck).Days > 0)
                {
                    Log.Information($"Day has passed!");
                    _lastCheck = DateTime.Now;
                    DateTime passedDay = _lastCheck.AddDays(-1);
                    SchedulePassed e = new SchedulePassed(Guid.NewGuid());
                    //await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
                }
                await Task.Delay(10000);
            }
        }






        public async Task<bool> HandleMessageAsync(string messageType, string message, IDictionary<string, object> userproperties)
        {
            try
            {
                JObject messageObject = MessageSerializer.Deserialize(message);
                switch (messageType)
                {
                    case "RealtimeWatcherAdded":
                        await HandleAsync(messageObject.ToObject<RealtimeWatcherAdded>());
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


        private async Task HandleAsync(RealtimeWatcherAdded watcheradded)
        {


            await Task.Delay(2000);

        }



    }
}
