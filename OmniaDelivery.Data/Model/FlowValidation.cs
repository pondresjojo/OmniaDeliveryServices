using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OmniaDelivery.Data.Model
{

    [Table("FlowValidation", Schema = "delivery")]
    public class FlowValidation
    {

        [Key()]
        public int ValidationId { get; set; }

        public int ConfigurationId { get; set; }

        public string Extension { get; set; }

        public string Filter { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public FlowConfiguration FlowConfiguration { get; set; }



    }
}
