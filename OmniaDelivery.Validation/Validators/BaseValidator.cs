using OmniaDelivery.Validation.Interfaces;
using OmniaDelivery.Validation.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.Validation.Validators
{
    public class BaseValidator: IValidator
    {
        protected ValidationRequest _request;

        public BaseValidator(ValidationRequest request)
        {

            _request = request;

        }

        public async virtual Task<ValidationResponse> Validate()
        {
            return await Task.FromResult<ValidationResponse>(new ValidationResponse() { Valid = false, Messages = new string[] { "Not Implemented Yet" } });

        }

    }
}
