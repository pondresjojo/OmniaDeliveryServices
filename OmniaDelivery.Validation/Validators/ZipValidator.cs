using OmniaDelivery.Validation.Interfaces;
using OmniaDelivery.Validation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.Validation.Validators
{
    public class ZipValidator : BaseValidator
    {


        public ZipValidator(ValidationRequest request) : base(request)
        {

        }


        public async override Task<ValidationResponse> Validate()
        {
            ValidationResponse response = new ValidationResponse();

            try
            {
                await Task.Run(() => {

                    using (var zipFile = ZipFile.OpenRead(_request.FileName))
                    {
                        var entries = zipFile.Entries;
                    }
                });

                response.Valid = true;


            }
            catch (InvalidDataException ex)
            {
                response.Valid = false;
                response.Messages = new string[] { ex.Message };
            }

            return response;
        }

    }
}
