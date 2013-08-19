using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace LambdaJamFSharpCSharpDemo
{
    class Program
    {
        public const string apiKey = "AIzaSyAUA6Pt5n4IyMlAc-5r2SM0SCuEUDHVsVg";

        [Serializable]
        class FictionalCharacter
        {
            public string type { get; set; }
            public string name { get; set; }
            public string[] character_created_by { get; set; }
            public string[] gender { get; set; }
            public string[] occupation { get; set; }
            public string[] species { get; set; }
            public string[] ethnicity { get; set; }
            public string[] married_to { get; set; }
            public string[] romantically_involved_with { get; set; }
            public string[] powers_or_abilities { get; set; }
            public string[] medical_conditions { get; set; }
            public string height { get; set; }
            public string weight { get; set; }
            public string[] appears_in_these_fictional_universes { get; set; }
            public string ToJsonString()
            {
                return "[" + JsonConvert.SerializeObject(this, Formatting.Indented) + "]\n";
            }
        }

            class FictionalCharacterContainer
            {
                //private List<FictionalCharacter> _result;
                //public List<FictionalCharacter> result
                //{
                //    get
                //    {
                //        if (_result == null)
                //        {
                //            _result = new List<FictionalCharacter>();
                //        }
                //        return _result;
                //    }

                //    set
                //    {
                //        _result = value;
                //    }
                //}
                public List<FictionalCharacter> result { get; set; }
                public FictionalCharacterContainer()
                {
                    result = new List<FictionalCharacter>();
                }
            }

        class FreeBaseReturn
        {
            public List<FictionalCharacter> result { get; set; }
            public string cursor { get; set; }
        }
        static void Main(string[] args)
        {
            var client = new RestClient();
            client.BaseUrl = "https://www.googleapis.com/freebase/v1/mqlread";
            var request = new RestRequest(Method.GET);
            request.AddParameter("key", apiKey);
            FictionalCharacter input = new FictionalCharacter
            {
                type = "/fictional_universe/fictional_character",
                character_created_by = new string[0],
                gender = new string[0],
                occupation = new string[0],
                species = new string[0],
                ethnicity = new string[0],
                married_to = new string[0],
                romantically_involved_with = new string[0],
                powers_or_abilities = new string[0],
                medical_conditions = new string[0],
                appears_in_these_fictional_universes = new string[0]
            };

            request.AddParameter("query", input.ToJsonString());
            request.AddParameter("cursor", string.Empty);
            request.RequestFormat = DataFormat.Json;
            var data = new FictionalCharacterContainer();
            string cursor = "";
            RestResponse response = null;
            while (cursor.ToLower() != "false")
            {
                response = (RestResponse)client.Execute(request);
                var retVal = JsonConvert.DeserializeObject<FreeBaseReturn>(response.Content);
                if(retVal.result != null)
                    data.result.AddRange(retVal.result);
                if (data.result.Count > 1000)
                    break;
                cursor = retVal.cursor;
                request.Parameters[2].Value = cursor;
            }
            //var result = data.result.OrderBy(r => r.name).ToList();
            var heroes = data
                        .result
                        .Where(d => d.powers_or_abilities.Count() > 0)
                        .Select(x => new { x.name, x.powers_or_abilities })
                        .ToList();
            var universes = data
                            .result
                            .Where(x => x.appears_in_these_fictional_universes.Count() > 0)
                            .Select(x => string.Join(", ", x.appears_in_these_fictional_universes))
                            .GroupBy(
                            .ToList();

            var powers = data.result.Where(x => x.powers_or_abilities.Count() > 0).Select(x => string.Join(", ", x.powers_or_abilities)).ToList();
            //Console.WriteLine("My output :" + "\n" + response.Content);
            //Console.WriteLine("My output :" + "\n" + JsonConvert.SerializeObject(data.result, Formatting.Indented));
            Console.ReadKey();
        }
    }
}
