using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using OmniaDelivery.Data;
using OmniaDelivery.Data.Model;
using OmniaDelivery.ValidationService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.ValidationService.Services
{
    public class DeliveryService : IDeliveryService
    {

        private DeliveryContext context;
        private readonly HttpClient _apiClient;
        private readonly string _documenturl;
        //private readonly string _forwardingurl;
        private readonly IOptions<AppSettings> _settings;




        public DeliveryService(DeliveryContext context, HttpClient httpClient)
        {
            this.context = context;
            _apiClient = httpClient;
            _documenturl = "https://localhost:5001/api/v1/document";
        }

        public async Task<bool> AddHeader()
        {

            try
            {
                var header = new DeliveryHeader()
                {
                    DeliveryId = Guid.NewGuid(),
                    ConfigurationID = 2,
                    Created = DateTime.Now
                };

                await context.DeliveryHeaders.AddAsync(header);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo: 
            }
            return false;


        }



        public async Task<bool> AddToStorage(Guid streamid, string content, string filename, string filetype, bool valid)
        {
            var dummyBuffer = Encoding.UTF8.GetBytes(content);
            var dummyStream = new MemoryStream(dummyBuffer);
            var inputData = new StreamContent(dummyStream);

            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            multiContent.Add(inputData, "file", filename);

            var response = await _apiClient.PostAsync(string.Format("{0}?streamId={1}&filename={2}&filetype={3}&isValid={4}", _documenturl, streamid, filename, filetype, valid), multiContent);

            return true;


        }




        public async Task<bool> AddHeader(Guid id, string flow)
        {

            var config = context.FlowConfigurations.Where(f => f.Flow == flow).FirstOrDefault();

            if (config == null) throw new Exception();

            try
            {
                var header = new DeliveryHeader()
                {
                    DeliveryId = id,
                    ConfigurationID = config.ConfigurationId,
                    Created = DateTime.Now
                };

                await context.DeliveryHeaders.AddAsync(header);
                await context.SaveChangesAsync();
                return true;


            }
            catch (Exception ex)
            {

            }
            return false;


        }


        public async Task<bool> AddFileExtension(Guid id, Guid streamid, string filename, bool valid, string additions = null, IList<ValueDTO> values = null)
        {

            try
            {
                var fileextension = new FileExtension()
                {

                    ProcessStreamId = streamid,
                    DeliveryId = id,
                    Extension = Path.GetExtension(filename),
                    Valid = valid,
                    StatusId = valid ? 10 : 5,
                    AdditionalInformation = additions,
                    Created = DateTime.Now

                    //                    NumberOfFiles = 1,
                    //                    StagingUUID = Guid.NewGuid(),
                    //                    Created = DateTime.Now,
                    //                   CustomerCode = "KLA0000",
                    //                   Flow = "LetterFlow"
                };

                if (values != null)
                {
                    fileextension.FileExtensionValues = values.Select(kvp => new FileExtensionValue() { Key = kvp.Key, Value = kvp.Value, Created = DateTime.Now }).ToList();
                    //fileextension.FileExtensionValues = new List<FileExtensionValue>();
                    //foreach (var v in values)
                    //{
                    //    var fev = new FileExtensionValue();
                    //    fev.Created = DateTime.Now;
                    //    fev.Key = v.Key;
                    //    //fev.Value = ((KeyValuePair<string, object>)v.Value).Value.ToString();

                    //}

                }



                await context.FileExtensions.AddAsync(fileextension);
                await context.SaveChangesAsync();
                return true;


            }
            catch (Exception ex)
            {

            }
            return false;


        }


        public async Task<DeliveryCompleted> CreateFowardingCommand(Guid id)
        {

            var config = context.DeliveryHeaders.Where(d => d.DeliveryId == id).Select(d => d.FlowConfiguration).FirstOrDefault();
            return new DeliveryCompleted(Guid.NewGuid(), id.ToString(), "KLA0102", config.Flow, config.NextService);
        }

        public async Task<ValidationResponse> Validate(string flow, string extension, string content)
        {

            ValidationResponse response = new ValidationResponse();
            var validationrule = context.FlowValidations.Where(fv => fv.FlowConfiguration.Flow == flow && fv.Extension == extension).FirstOrDefault();
            if (validationrule == null)
            {
                response.Valid = false;
                response.Messages = new string[] { "no rule found" };
            }
            else
            {
                JSchema schema = JSchema.Parse(validationrule.Content);

                try
                {

                    var result = await Task.Run(() =>
                    {

                        JObject a1 = JObject.Parse(content);
                        IList<string> messages;
                        bool valid = a1.IsValid(schema, out messages);
                        return messages;
                    });

                    response.Valid = result.Count == 0;
                    response.Messages = result.ToArray();

                }
                catch (Exception ex)
                {
                    response.Valid = false;
                    response.Messages = new string[] { ex.Message };
                }


            }

            return response;

        }
    }
}
