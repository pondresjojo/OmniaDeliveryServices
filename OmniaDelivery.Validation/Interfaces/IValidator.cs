using OmniaDelivery.Validation.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.Validation.Interfaces
{
    public interface IValidator
    {
        Task<ValidationResponse> Validate();

    }
}
