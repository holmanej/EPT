using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static EVE_Production_Tool.ESI;
using static EVE_Production_Tool.AssetLUT;
using Newtonsoft.Json;

namespace EVE_Production_Tool
{
    static class MarketBrowser
    {

        //public struct RefinedOrder
        //{
        //    public string SystemName { get; set; }
        //    public string TargetSystem { get; set; }
        //    public string Distance { get; set; }
        //    public string Price { get; set; }
        //    public string Quantity { get; set; }
        //}

        //public static async Task<List<RefinedOrder>> BrowseMarketAsync(List<string> regionIDs, string origin, string typeID, string orderType)
        //{
            //List<RefinedOrder> orders_found = new List<RefinedOrder>();
            //List<MarketOrder> ordersList = new List<MarketOrder>();

            //foreach (string regionID in regionIDs)
            //{
            //    ordersList.AddRange(await GetOrders(regionID, typeID, orderType));
            //    Console.WriteLine(regionIDs.IndexOf(regionID) + "/" + regionIDs.Count);
            //};

            //foreach (Order_Entry order in ordersList)
            //{
            //    HttpResponseMessage response = await GetRouteContent(origin, order.system_id);
            //    if (response != null)
            //    {
            //        List<string> route = await DeserializeRouteResponse(response);
            //        int distance = ConvertRange(order.range) > route.Count ? 0 : route.Count - ConvertRange(order.range);
            //        route.Insert(0, origin);
            //        //Console.WriteLine("Sys: " + FindSystemName(order.system_id) + "  D: " + distance + "  P: " + order.price + "  Q: " + order.volume_remain + "  Tsys: " + FindSystemName(route[distance]));
            //        tempOrder.SystemName = FindSystemName(order.system_id);
            //        tempOrder.TargetSystem = FindSystemName(route[distance]);
            //        tempOrder.Distance = distance.ToString();
            //        tempOrder.Price = order.price.ToString();
            //        tempOrder.Quantity = order.volume_remain.ToString();
            //        orders_found.Add(tempOrder);
            //    }
            //    else
            //    {
            //        Console.WriteLine("get route failed");
            //    }
            //};
            //return orders_found;
        //}

        
    }
}
