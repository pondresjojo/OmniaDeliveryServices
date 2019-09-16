using OmniaDelivery.InputProcessingService.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.InputProcessingService.Services
{
    public interface ISingleService
    {


        Task<DeliveryCompleted> ProcessSingleFile(FileDelivered filedeliveredcommand);


    }
}
