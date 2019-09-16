using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using OmniaDelivery.Infra.Helpers;
using OmniaDelivery.Infra.Interfaces;
using OmniaDelivery.WatchService.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OmniaDelivery.WatchService
{
    public class WatchManager : IHostedService, IMessageHandlerCallback
    {

        IMessageHandler _messageHandler;
        IMessagePublisher _messagePublisher;
        public WatchManager(IMessageHandler messageHandler, IMessagePublisher messagepublisher)
        {
            _messageHandler = messageHandler;
            _messagePublisher = messagepublisher;
        }

        public async Task<bool> HandleMessageAsync(string messageType, string message, IDictionary<string, object> userproperties)
        {

                try
                {
                    JObject messageObject = MessageSerializer.Deserialize(message);
                    switch (messageType)
                    {
                        case "RealtimeTriggered":
                            await HandleAsync(messageObject.ToObject<RealtimeTriggered>());
                            break;
                        case "SchedulePassed":
                            await HandleAsync(messageObject.ToObject<SchedulePassed>());
                            break;
                    case "TriggerTriggered":
                        await HandleAsync(messageObject.ToObject<SchedulePassed>());
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


        private async Task HandleAsync(RealtimeTriggered realtimetriggered)
        {

            //ophalen alles bestanden die voldoen aan de trigger
            //bestanden verplaatsten naar inprocess map (in mapje met id).
            //dan de volgende queue aanroepen voor de echte verwerking
            var files = Directory.GetFiles(realtimetriggered.Folder, realtimetriggered.Filter);
            foreach(var file in files)
            {
                //kijken of file gelocked is, zo ja dan overslaan
                File.Move(file, Path.Combine(Path.GetDirectoryName(file), "InProcess", Path.GetFileName(file)));
                FileDelivered e = new FileDelivered(realtimetriggered.MessageId, Guid.NewGuid(), Path.Combine(Path.GetDirectoryName(file), "InProcess", Path.GetFileName(file)), realtimetriggered.Flow, realtimetriggered.Customer);
                await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
            }
            //await Task.Delay(2000);

        }

        private async Task HandleAsync(SchedulePassed schedulepassed)
        {


            await Task.Delay(2000);

        }

        private async Task HandleAsync(TriggerTriggered triggertriggered)
        {

            await Task.Delay(2000);

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
