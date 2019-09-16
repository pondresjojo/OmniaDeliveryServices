using System;
using System.Collections.Generic;
using System.Text;

namespace OmniaDelivery.InputProcessingService.Model
{
    public class ValidationItem
    {

        public Guid Id { get; set; }
        public string Customer { get; set; }
        public string Flow { get; set; }
        public string Content { get; set; }
        public IList<ValueDTO> Values { get; set; }

    }


    public class ValueDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }

    }
}
