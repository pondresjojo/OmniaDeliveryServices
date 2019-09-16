using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.Infra.Interfaces
{
    public interface IMessageHandler
    {

        void Start(IMessageHandlerCallback callback);
        void Stop();

    }
}
