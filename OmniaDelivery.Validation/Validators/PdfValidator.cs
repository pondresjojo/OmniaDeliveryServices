using iTextSharp.text.pdf;
using OmniaDelivery.Validation.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.Validation.Validators
{
    public class PdfValidator : BaseValidator
    {


        public PdfValidator(ValidationRequest request) : base(request)
        {

        }


        public async override Task<ValidationResponse> Validate()
        {
            ValidationResponse response = new ValidationResponse();

            try
            {

                var result = await Task.Run(() =>
                {
                    PdfReader pdfReader = new PdfReader(_request.FileName);
                    int pages = pdfReader.NumberOfPages;
                    pdfReader.Close();
                    return pages;

                });

                response.Valid = result > 0;

            }
            catch (Exception ex)
            {
                response.Valid = false;
                response.Messages = new string[] { ex.Message };
            }

            return response;
        }

    }
}
