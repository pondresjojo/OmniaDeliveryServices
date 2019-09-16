using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.Validation.Models
{
    public class ValidationRequest
    {
        public string FileName { get; set; }

        public string Flow { get; set; }

        public string Customer { get; set; }


        public ValidationRule Rule { get; set; }


    }
}
