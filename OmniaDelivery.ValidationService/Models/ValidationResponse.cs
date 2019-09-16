using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.ValidationService.Models
{
    public class ValidationResponse
    {
        public Guid ReferenceId { get; set; }
        public bool Valid { get; set; }

        public string Code { get; set; }
        public string[] Messages { get; set; }
    }
}
