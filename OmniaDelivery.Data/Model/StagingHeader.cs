using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OmniaDelivery.Data.Model
{

    [Table("StagingHeader", Schema = "delivery")]
    public class StagingHeader
    {
        [Key()]

        public int StagingHeaderId { get; set; }
        public int? ParentStagingHeaderId { get; set; }
        public Guid StagingUUID { get; set; }
        public int NumberOfFiles { get; set; }
        public string CustomerCode { get; set; }
        public string Flow { get; set; }
        public DateTime Created { get; set; }

        public virtual ICollection<StagingExtension> StagingExtensions { get; set; }






    }
}
