using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OmniaDelivery.InputProcessingService.Workflow
{
    public class SingleFile
    {

        public SingleFile(Guid deliveryid)
        {

            DeliveryId = deliveryid;
            Messages = new List<string>();
            SingleFiles = new List<SingleFile>();
        }

        public Guid DeliveryId { get; internal set; }

        public Guid? ParentId { get; internal set; }

        public string FileName { get; set; }

        public string SubFolder { get; set; }

        public string Flow { get; set; }

        public string Customer { get; set; }

        public bool Valid { get; set; }

        public List<string> Messages { get; set; }

        public bool Saved { get; set; }

        public bool IsArchive
        {
            get
            {
                return GetExtension() == ".zip";
            }
        }


        public IList<SingleFile> SingleFiles { get; set; }

        public string GetExtension()
        {
            return Path.GetExtension(this.FileName);
        }

        public string GetFileType()
        {
            return Path.GetExtension(this.FileName).Replace(".","");
        }


    }
}
