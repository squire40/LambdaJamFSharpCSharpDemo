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
            public string gender { get; set; }
            public string[] occupation { get; set; }
            public string[] species { get; set; }
            public string ethnicity { get; set; }
            public string[] married_to { get; set; }
            public string[] romantically_involved_with { get; set; }
            public string[] powers_or_abilities { get; set; }
            public string[] medical_conditions { get; set; }
            public string height { get; set; }
            public string weight { get; set; }
            //public int limit { get; set; }
            public string ToJsonString()
            {
                return "[" + JsonConvert.SerializeObject(this, Formatting.Indented) + "]\n";
            }
        }

        [Serializable]
        class FictionalCharacterContainer
        {
            private List<FictionalCharacter> _result;
            public List<FictionalCharacter> result 
            { 
                get
                {
                    if (_result == null)
	                {
		                    _result = new List<FictionalCharacter>();
	                }
                    return _result;
                }
            
                set
                {
                    _result = value;
                }
            }
            public FictionalCharacterContainer()
            {
                _result = new List<FictionalCharacter>();
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
                occupation = new string[0],
                species = new string[0],
                married_to = new string[0],
                romantically_involved_with = new string[0],
                powers_or_abilities = new string[0],
                medical_conditions = new string[0]
            };

            request.AddParameter("query", input.ToJsonString());
            //request.AddParameter("limit", 10);
            request.AddParameter("cursor", string.Empty);
            request.RequestFormat = DataFormat.Json;
            var data = new FictionalCharacterContainer();
            string cursor = "";
            RestResponse response = null;
            while (cursor.ToLower() != "false")
            {
                response = (RestResponse)client.Execute(request);
                //var retVal = JsonConvert.DeserializeObject<FictionalCharacterContainer>(response.Content);
                var retVal = JsonConvert.DeserializeObject<FreeBaseReturn>(response.Content);
                //FreeBaseReturn
                data.result.AddRange(retVal.result);
                cursor = retVal.cursor;
                Console.WriteLine(cursor);
                //request.AddParameter("cursor", cursor);
                request.Parameters[2].Value = cursor;
            }
            //var result = data.result.OrderBy(r => r.name).ToList();
            //var heroes = data.result.Where(d => d.powers_or_abilities.Count() > 0).ToList();

            Console.WriteLine("My output :" + "\n" + response.Content);
            Console.ReadKey();
        }
    }
}
