using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmniaDelivery.InputProcessingService.Extensions;
using OmniaDelivery.InputProcessingService.Model;
using OmniaDelivery.InputProcessingService.Workflow;
using OmniaDelivery.Validation.Models;

namespace OmniaDelivery.InputProcessingService.Services
{
    public class SingleService: ISingleService
    {

        private IDeliveryService _service;

        public SingleService(IDeliveryService deliveryservice)
        {
            _service = deliveryservice;
        }

        public async Task<DeliveryCompleted> ProcessSingleFile(FileDelivered filedeliveredcommand)
        {
            SingleFile singlefile = new SingleFile(filedeliveredcommand.DeliveryId)
            {
                Customer = filedeliveredcommand.Customer,
                FileName = filedeliveredcommand.FileName,
                Flow = filedeliveredcommand.Flow,
            };


            await PreProcessFile(singlefile);
            await ProcessFile(singlefile);
            await PostProcessFile(singlefile);
            //Guid headerid = Guid.NewGuid();

            //create header
            //await _service.AddHeader(singlefile.DeliveryId, singlefile.Flow);
            //save files recursief



            //recursief opslaan


            //beastanden verwijderen

            //ophalen vervolgacties???


            //notificeren

            return new DeliveryCompleted(Guid.NewGuid(), singlefile.DeliveryId.ToString(), singlefile.Customer, singlefile.Flow, "");


        }


        private async Task PreProcessFile(SingleFile file)
        {

            var request = new ValidationRequest() { FileName = file.FileName, Flow = file.Flow };
            var validateresponse = await _service.Validate(request);
            file.Valid = validateresponse.Valid;
            if(validateresponse.Messages != null) file.Messages.AddRange(validateresponse.Messages);
            if(file.Valid && file.IsArchive)
            {

                var templocation = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(file.FileName));
                Directory.CreateDirectory(templocation);
                using (ZipArchive archive = ZipFile.OpenRead(file.FileName))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string extractedfile = Path.Combine(templocation, entry.FullName);
                        entry.ExtractToFile(extractedfile);
                        file.SingleFiles.Add(new SingleFile(Guid.NewGuid()) { Flow = file.Flow, FileName = extractedfile, ParentId = file.DeliveryId, SubFolder = Path.GetFileNameWithoutExtension(file.FileName) });
                    }
                }
            }
            foreach (var sf in file.SingleFiles)
            {
                await PreProcessFile(sf);
            }
        }



        private async Task ProcessFile(SingleFile file)
        {


            Guid streamid = Guid.NewGuid();
            if(await _service.AddToStorage(streamid, file.FileName, file.GetFileType(), file.Valid, file.SubFolder))
            {
                if(await _service.AddHeader(file.DeliveryId, file.Flow, file.ParentId))
                {
                    if(await _service.AddFileExtension(file.DeliveryId, streamid, Path.GetFileName(file.FileName), file.Valid, string.Join("\n", file.Messages), null))
                    {
                        foreach (var sf in file.SingleFiles)
                        {
                            await ProcessFile(sf);
                        }
                        file.Saved = true;
                    }
                }
            }


        }



        private async Task PostProcessFile(SingleFile file)
        {


            FileInfo fi = new FileInfo(file.FileName);
            await fi.DeleteAsync();
            foreach(var sf in file.SingleFiles)
            {
                await PostProcessFile(sf);
            }
            if (file.IsArchive)
            {
                DirectoryInfo di = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(file.FileName)));
                if (di.GetFiles().Count() == 0) di.Delete();
            }

        }




    }
}
