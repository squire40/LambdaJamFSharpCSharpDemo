using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace LambdaJamFSharpCSharpDemo
{
    class Program
    {
        public const string apiKey = "AIzaSyAUA6Pt5n4IyMlAc-5r2SM0SCuEUDHVsVg";

        class Constellation
        {
            public string type { get; set; }
            public string[] name { get; set; }
            public string[] category { get; set; }
            public string[] celestial_age { get; set; }
            //public string[] magnitude { get; set; }
            //public string[] decllnation { get; set; }
            //public string[] right_ascention { get; set; }
            public string ToJsonString()
            {
                return "[" + JsonConvert.SerializeObject(this, Formatting.Indented) + "]\n";
            }
        }

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
            public string ToJsonString()
            {
                return "[" + JsonConvert.SerializeObject(this, Formatting.Indented) + "]\n";
            }
        }

        [Serializable]
        class Wha
        {
            public List<FictionalCharacter> result { get; set; }
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
            request.RequestFormat = DataFormat.Json;
            RestResponse response = (RestResponse)client.Execute(request);
            
            // var data = JsonConvert.DeserializeObject<List<FictionalCharacter>>(response.Content);
            var data = response.Content.FromJson<Wha>();

            Console.WriteLine("My output :" + "\n" + response.Content);
            Console.ReadKey();
        }
    }
}
