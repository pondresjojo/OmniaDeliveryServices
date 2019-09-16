using OmniaDelivery.Validation.Interfaces;
using OmniaDelivery.Validation.Models;
using OmniaDelivery.Validation.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OmniaDelivery.Validation
{
    public static class ValidatorFactory
    {

        public static IValidator CreateValidator(ValidationRequest request)
        {

            IValidator validator;

            switch (Path.GetExtension(request.FileName).ToLower())
            {
                case ".zip":
                    validator = new ZipValidator(request);
                    break;
                case ".pdf":
                    validator = new PdfValidator(request);
                    break;
                case ".xml":
                    validator = new XmlValidator(request);
                    break;
                case ".doc":
                case ".docx":
                    validator = new WordValidator(request);
                    break;
                case ".json":
                    validator = new JsonValidator(request);
                    break;
                case ".txt":
                    validator = new TextValidator(request);
                    break;
                case ".csv":
                    validator = new CsvValidator(request);
                    break;
                case ".html":
                case ".htm":
                    validator = new HtmlValidator(request);
                    break;
                default:
                    validator = new UnknownValidator(request);
                    break;
            }

            return validator;

        }



    }
}
