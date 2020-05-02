using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EveMarketWatcher {
    public class OrderList {

        public class Order {
            public int duration { get; set; }
            public bool is_buy_order { get; set; }
            public DateTime issued { get; set; }
            public string location_id { get; set; }
            public int min_volume { get; set; }
            public string order_id { get; set; }
            public double price { get; set; }
            public string range { get; set; }
            public string system_id { get; set; }
            public int type_id { get; set; }
            public int volume_remain { get; set; }
            public int volume_total { get; set; }
        }

        static string baseUrl = "https://esi.evetech.net/dev/markets/";
        static HttpClient client = new HttpClient();
        public List<Order> orders;

        public async Task<string> GetOrderAsync(string orderType, string item, string system) {
            string path = baseUrl + system + "/orders/?datasource=tranquility&order_type=" + orderType + "&page=1&type_id=" + item;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode) {
                orders = JsonConvert.DeserializeObject<List<Order>>(await response.Content.ReadAsStringAsync());
            }
            else {
                Console.WriteLine("Error: " + response.StatusCode);
            }
            return response.StatusCode.ToString();
        }

        public double GetHigh90th() {
            double max = orders.Max(t => t.price);
            double min = orders.Min(t => t.price);
            Console.WriteLine(max - (0.1 * (max - min)));
            return max - (0.1 * (max - min));
        }

        public double GetLow90th() {
            double max = orders.Max(t => t.price);
            double min = orders.Min(t => t.price);
            return min + (0.1 * (max - min));
        }
    }
}