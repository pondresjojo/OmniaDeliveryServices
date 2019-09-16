using OmniaDelivery.Validation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace OmniaDelivery.Validation.Validators
{
    public class XmlValidator : BaseValidator
    {

        public XmlValidator(ValidationRequest request) : base(request)
        {

        }


        public async override Task<ValidationResponse> Validate()
        {

            ValidationResponse response = new ValidationResponse();

            XmlSchemaSet schemas = new XmlSchemaSet();

            //schemas.Add("", @"C:\Temp\centric\XSD\KLA000_FLOW1.xsd");
            schemas.Add("", XmlReader.Create(new StringReader(this._request.Rule.Content)));

            try
            {

                var result = await Task.Run(() =>
                {

                    List<string> msg = new List<string>();
                    try
                    {
                        XDocument doc = XDocument.Load(_request.FileName);
                        doc.Validate(schemas, (o, e) =>
                        {
                            msg.Add(e.Message);
                        });

                    }
                    catch (Exception ex)
                    {
                        msg.Add(ex.Message);
                    }
                    return msg;
                });



                response.Valid = result.Count == 0;
                response.Messages = result.ToArray();

            }
            catch (Exception ex)
            {
                response.Valid = false;
                response.Messages = new string[] { ex.Message };
            }

            return response;
        }


    }
}
