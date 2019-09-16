using OmniaDelivery.ValidationService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.ValidationService.Services
{
    public interface IDeliveryService
    {

        Task<bool> AddHeader(Guid id, string flow);

        Task<bool> AddFileExtension(Guid id, Guid streamid, string filename, bool valid, string additions = null, IList<ValueDTO> values = null);

        Task<bool> AddToStorage(Guid streamid, string content, string filename, string filetype, bool valid);

        Task<DeliveryCompleted> CreateFowardingCommand(Guid id);

        Task<ValidationResponse> Validate(string flow, string extension, string content);


    }
}
