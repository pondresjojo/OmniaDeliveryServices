using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.Validation.Models
{
    public class ValidationResponse
    {

        public ValidationResponse()
        {
        }


        public bool Valid { get; set; }

        public string[] Messages { get; set; }

    }
}
