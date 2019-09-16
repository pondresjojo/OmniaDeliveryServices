using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OmniaDelivery.Data.Model
{

    [Table("FileExtensionValue", Schema = "delivery")]
    public class FileExtensionValue
    {

        [Key()]
        public int FileExtensionValueId { get; set; }

        public int FileExtensionId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }


        public DateTime Created { get; set; }

        public FileExtension FileExtension { get; set; }

    }
}
