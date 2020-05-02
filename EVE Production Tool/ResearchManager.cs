using EveMarketWatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EVE_Production_Tool.AssetLUT;

namespace EVE_Production_Tool
{
    class ResearchManager
    {
        public static async Task GetPopularT2ItemsAsync()
        {
            string[] itemList = System.IO.File.ReadAllLines(@"ItemLUTfile.txt");
            List<KeyValuePair<string, int>> ItemCount = new List<KeyValuePair<string, int>>();
            MarketOrder_List orders = new MarketOrder_List()
            {
                OrderType = "buy"
            };
            int progress = 0;
            foreach (string item in itemList)
            {
                progress++;
                string[] parts = item.Split(',');
                //if (item.Contains("II") && !item.Contains("Blueprint"))
                if ((parts[2] == "192" || parts[2] == "184" || parts[2] == "189") && !(parts[1] == "Blueprint"))
                {
                    orders.TypeID = parts[0];
                    await orders.GetOrders(FindRegionID("The Forge"));
                    ItemCount.Add(new KeyValuePair<string, int>(parts[1], orders.Count));
                    orders.Clear();
                    Console.WriteLine(progress + "/" + itemList.Count() + "  Name: " + parts[1]);
                }
            }
            ItemCount = ItemCount.OrderByDescending(q => q.Value).ToList();
            for (int i = 0; i < ItemCount.Count; i++)
            {
                Console.WriteLine("Name: " + ItemCount[i].Key + "  Q: " + ItemCount[i].Value);
            }
        }
    }
}
