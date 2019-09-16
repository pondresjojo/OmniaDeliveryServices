using OmniaDelivery.Data;
using OmniaDelivery.TimeService.Events;
using OmniaDelivery.TimeService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.TimeService.Services
{
    public class RealtimeService: ITimerService
    {

        private DeliveryContext context;
        private IList<RealtimeWatcher> watchers;


        public RealtimeService(DeliveryContext context)
        {
            this.context = context;
            watchers = new List<RealtimeWatcher>();
        }

        public async Task InitAsync()
        {
            await Task.Delay(1000);
        }

        public async Task<List<RealtimeTriggered>> Process()
        {

            var items = new List<RealtimeTriggered>();

            RealtimeTriggered e = new RealtimeTriggered(Guid.NewGuid());
            e.Flow = "CardsFlow";
            e.Customer = "KLA0000";
            e.Folder = @"C:\Temp\Omnia\Realtime";
            e.Filter = "*.zip";
            items.Add(e);

            return items;

        }

        public async Task Refresh()
        {
            await Task.Delay(1000);
        }
    }
}
