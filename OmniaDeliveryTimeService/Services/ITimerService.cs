using OmniaDelivery.TimeService.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.TimeService.Services
{
    public interface ITimerService
    {

        Task InitAsync();

        Task<List<RealtimeTriggered>> Process();

        Task Refresh();


    }
}
