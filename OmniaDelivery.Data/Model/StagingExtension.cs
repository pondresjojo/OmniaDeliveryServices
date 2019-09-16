using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OmniaDelivery.Data.Model
{
    [Table("StagingExtension", Schema = "delivery")]

    public class StagingExtension
    {

        [Key()]

        public int StagingExtensionId { get; set; }

        public int HeaderId { get; set; }

        public Guid StreamId { get; set; }

        public int StatusId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime Created { get; set; }

        public StagingHeader StagingHeader { get; set; }




    }
}
