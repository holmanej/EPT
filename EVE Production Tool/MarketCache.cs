using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EVE_Production_Tool
{
    class MarketCache
    {
        //public Dictionary<int, MarketOrder> SystemOrders = new Dictionary<int, MarketOrder>();
        private List<MarketOrder> Orders = new List<MarketOrder>();

        public MarketCache()
        {
            List<int> regionList = new List<int>();
            foreach (string line in System.IO.File.ReadAllLines(@"RegionLUTfile.txt"))
            {
                if (line.StartsWith("10"))
                {
                    regionList.Add(int.Parse(line.Substring(0, 8)));
                }
            }
            string orderType = "sell";
            List<int> regionDone = new List<int>();
            regionList.ForEach(async delegate (int region)
            {
                int pages = await AsyncOrdering(region, orderType, 1);
                for (int i = 2; i <= pages; i++)
                {
                    await AsyncOrdering(region, orderType, i);
                }
                regionDone.Add(region);
                Debug.WriteLine(regionDone.Count + "/" + regionList.Count + "  " + region + " : " + Orders.Count);
                Debug.WriteLine(Process.GetCurrentProcess().Threads.Count);
                //foreach (int r in regionList.FindAll(i => !regionDone.Exists(j => j == i)))
                //{
                //    Debug.WriteLine(r);
                //}
            });
            Debug.WriteLine("done");
        }

        private async Task<int> AsyncOrdering(int regionID, string orderType, int pageIndex)
        {
            HttpClient client = new HttpClient();
            string path = "https://esi.evetech.net/latest/markets/" + regionID + "/orders/?datasource=tranquility&order_type=" + orderType + "&page=" + pageIndex;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                if (response != null)
                {
                    int pages = int.Parse(response.Headers.GetValues("x-pages").First());
                    Orders.AddRange(JsonConvert.DeserializeObject<List<MarketOrder>>(response.Content.ReadAsStringAsync().Result));
                    return pages;
                }
                else
                {
                    Debug.WriteLine("Error: GetOrders deserialize failed");
                }
            }
            else
            {
                Debug.WriteLine("GetOrders Response error: " + response.StatusCode + " : " + regionID + " - " + pageIndex);
            }
            return 0;
        }
    }
}
