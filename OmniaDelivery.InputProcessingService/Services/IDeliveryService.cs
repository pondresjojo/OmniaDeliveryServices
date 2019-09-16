using OmniaDelivery.InputProcessingService.Model;
using OmniaDelivery.Validation.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.InputProcessingService.Services
{
    public interface IDeliveryService
    {

        Task<bool> AddHeader(Guid id, string flow, Guid? parentid);

        Task<bool> AddFileExtension(Guid id, Guid streamid, string filename, bool valid, string additions = null, IList<ValueDTO> values = null);

        Task<bool> AddToStorage(Guid streamid, string filename, string filetype, bool valid, string subfolder);

        Task<DeliveryCompleted> CreateFowardingCommand(Guid id);

        Task<ValidationResponse> Validate(ValidationRequest request);


    }
}
