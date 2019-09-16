using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using OmniaDelivery.Data;
using OmniaDelivery.Data.Model;
using OmniaDelivery.InputProcessingService.Model;
using OmniaDelivery.Validation;
using OmniaDelivery.Validation.Interfaces;
using OmniaDelivery.Validation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OmniaDelivery.InputProcessingService.Services
{
    public class DeliveryService : IDeliveryService
    {

        private DeliveryContext context;
        private readonly HttpClient _apiClient;
        private readonly string _documenturl;

        public DeliveryService(DeliveryContext context, HttpClient httpClient)
        {
            this.context = context;
            _apiClient = httpClient;
            //_documenturl = $"{_settings.Value.DocumentUrl}/api/v1/document";
            _documenturl = "https://localhost:5001/api/v1/document";
            //_forwardingurl = $"{_settings.Value.ForwardingUrl}/api/v1/forwardings";
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

            }
            return false;


        }



        public async Task<bool> AddToStorage(Guid streamid, string filename, string filetype, bool valid, string subfolder)
        {

            using (var multicontent = new MultipartFormDataContent())
            using (var stream = File.OpenRead(filename))
            using (var streamContent = new StreamContent(stream))
            {
                multicontent.Add(streamContent, "file", Path.GetFileName(filename));
                var response = await _apiClient.PostAsync(string.Format("{0}?streamId={1}&filename={2}&filetype={3}&isValid={4}&subfolder={5}", _documenturl, streamid, Path.GetFileName(filename), filetype, valid, subfolder), multicontent);
                return true;
            }

        }




        public async Task<bool> AddHeader(Guid id, string flow, Guid? parentid)
        {

            var config = context.FlowConfigurations.Where(f => f.Flow == flow).FirstOrDefault();

            if (config == null) throw new Exception();

            try
            {
                var header = new DeliveryHeader()
                {
                    DeliveryId = id,
                    ParentDeliveryId = parentid,
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
                };

                if (values != null)
                {
                    fileextension.FileExtensionValues = values.Select(kvp => new FileExtensionValue() { Key = kvp.Key, Value = kvp.Value, Created = DateTime.Now }).ToList();
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

        public async Task<ValidationResponse> Validate(ValidationRequest request)
        {


            ValidationResponse response = new ValidationResponse();

            foreach (var valrule in context.FlowValidations.Where(r => r.FlowConfiguration.Flow == request.Flow && r.Extension == Path.GetExtension(request.FileName).ToLower()))
            {
                Match m = Regex.Match(Path.GetFileName(request.FileName), valrule.Filter, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    request.Rule = new ValidationRule() { Content = valrule.Content, Extensie = valrule.Extension, Filter = valrule.Filter };
                    break;
                }

            }

            if (request.Rule == null)
            {
                response.Valid = false;
                response.Messages = new string[] { "no rule found" };
            }
            else
            {

                IValidator validator = ValidatorFactory.CreateValidator(request);
                response = await validator.Validate();
            }

            return response;

        }
    }
}
