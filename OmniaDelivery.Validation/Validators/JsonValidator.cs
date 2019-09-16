using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using OmniaDelivery.Validation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniaDelivery.Validation.Validators
{
    public class JsonValidator: BaseValidator
    {


        public JsonValidator(ValidationRequest request) : base(request)
        {

        }


        public async override Task<ValidationResponse> Validate()
        {

            ValidationResponse response = new ValidationResponse();

            JSchema schema = JSchema.Parse(this._request.Rule.Content);

            try
            {

                var result = await Task.Run(() =>
                {

                    JArray a1 = JArray.Parse(File.ReadAllText(_request.FileName));
                    //JObject o1 = JObject.Parse("[{\"AchternaamGeadresseerde\":\"Vries\",\"AdresKantoor\":\"Hoofdstraat 10\",\"ExtraVeld1\":\"\",\"ExtraVeld10\":\"\",\"ExtraVeld2\":\"\",\"ExtraVeld3\":\"\",\"ExtraVeld4\":\"\",\"ExtraVeld5\":\"\",\"ExtraVeld6\":\"\",\"ExtraVeld7\":\"\",\"ExtraVeld8\":\"\",\"ExtraVeld9\":\"\",\"GeslachtGeaddresseerde\":1,\"GeslachtKind\":0,\"HuisnummerGeaddresseerde\":\"11\",\"LandGeaddresseerde\":\"Nederland\",\"NaamKantoor\":\"Monuta Apeldoorn\",\"NaamKind\":\"Anna\",\"Naamuitvaartverzorger\":\"Jansen\",\"PersoonlijkeCode\":\"HEfOcwLg\",\"PlaatsGeaddresseerde\":\"Apeldoorn\",\"PlaatsKantoor\":\"Apeldoorn\",\"PostcodeGeaddresseerde\":\"1234 AB\",\"Referentienummer\":\"1234567\",\"StraatGeaddresseerde\":\"Lindelaan\",\"StuurcodeBrief\":\"ABCD123\",\"TelefoonnrGeaddresseerde\":\"0800 0230550\",\"ToevoegingGeaddresseerde\":\"a\",\"TussenvoegselGeadresseerde\":\"de\",\"VoorlettersGeaddresseerde\":\"J.M.\",\"VoornaamGeaddresseerde\":\"Jan\"}]");
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

            return response;
        }


    }
}
