using OmniaDelivery.Validation.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.Validation.Validators
{
    public class UnknownValidator : BaseValidator
    {


        public UnknownValidator(ValidationRequest request) : base(request)
        {

        }
        public async override Task<ValidationResponse> Validate()
        {
            throw new NotImplementedException();
        }
    }
}
