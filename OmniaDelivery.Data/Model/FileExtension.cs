using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OmniaDelivery.Data.Model
{
    [Table("FileExtension", Schema = "delivery")]

    public class FileExtension
    {

        [Key()]
        public int FileExtensionId { get; set; }

        public Guid ProcessStreamId { get; set; }

        public Guid DeliveryId { get; set; }

        public bool Valid  { get; set; }
        public string Extension { get; set; }


        public string AdditionalInformation { get; set; }

        public int StatusId { get; set; }
        public DateTime Created { get; set; }

        public DeliveryHeader DeliveryHeader { get; set; }

        public virtual ICollection<FileExtensionValue> FileExtensionValues { get; set; }




    }
}
