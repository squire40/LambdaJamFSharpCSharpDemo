using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaJamFSharpCSharpDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient();
            client.BaseUrl = "https://www.freebase.com/api/service/mqlread";
            var request = new RestRequest(Method.GET);
        }
    }
}
