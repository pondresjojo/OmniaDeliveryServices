using OmniaDelivery.Validation.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.Validation.Validators
{
    public class WordValidator : BaseValidator
    {


        public WordValidator(ValidationRequest request) : base(request)
        {

        }


        public override Task<ValidationResponse> Validate()
        {
            ValidationResponse response = new ValidationResponse();
            response.Valid = true;
            return Task.FromResult<ValidationResponse>(response);
        }

    }
}
