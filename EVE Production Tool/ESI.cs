using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EVE_Production_Tool
{

    static class ESI
    {

        public class RootObject
        {
            public string Error { get; set; }
        }

        static readonly string baseUrl = "https://esi.evetech.net/latest";
        static readonly HttpClient client = new HttpClient();

        public static async Task<List<string>> DeserializeRouteResponse(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<HttpResponseMessage> GetRouteContent(string origin, string dest)
        {
            string path = baseUrl + "/route/" + origin + "/" + dest + "/?datasource=tranquility&flag=secure";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                //Console.WriteLine("Route success" + response.StatusCode);
                return response;
            }
            else
            {
                Console.WriteLine("Route Response error: " + response.StatusCode);
                //RootObject body = JsonConvert.DeserializeObject<RootObject>(await response.Content.ReadAsStringAsync());
                //Console.WriteLine(body.error);
                return null;
            }            
        }

        public static int ConvertRange(string range)
        {
            if (int.TryParse(range, out int distance))
            {
                return distance;
            }
            else
            {
                return 0;
            }
        }

    }
}
