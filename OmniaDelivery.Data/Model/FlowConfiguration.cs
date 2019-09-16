using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OmniaDelivery.Data.Model
{
    [Table("FlowConfiguration", Schema = "delivery")]
    public class FlowConfiguration
    {
        [Key()]

        public int ConfigurationId { get; set; }

        public string Flow { get; set; }

        public bool SubmitIncomplete { get;set; }
        public bool SendMailOnError { get; set; }

        public string EndpointType { get; set; }
        public string Endpoint { get; set; }
        public string QueueName { get; set; }
        public string NextService { get; set; }
        public string MailAddress { get; set; }


        public virtual ICollection<DeliveryHeader> DeliveryHeaders { get; set; }

        public virtual ICollection<FlowValidation> FlowValidations { get; set; }


    }
}
