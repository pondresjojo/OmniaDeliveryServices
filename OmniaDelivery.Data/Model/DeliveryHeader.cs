using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OmniaDelivery.Data.Model
{

    [Table("DeliveryHeader", Schema = "delivery")]
    public class DeliveryHeader
    {
        [Key()]

        public Guid DeliveryId { get; set; }

        public int ConfigurationID { get; set; }
        public Guid? ParentDeliveryId { get; set; }
        public DateTime Created { get; set; }

        public virtual ICollection<FileExtension> FileExtensions { get; set; }
        public FlowConfiguration FlowConfiguration { get; set; }





    }
}
