﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static EVE_Production_Tool.ESI;

namespace EVE_Production_Tool
{
    class MarketOrder_List : List<MarketOrder>
    {
        public string TypeID { get; set; }
        public string OrderType { get; set; }
        public string OriginSystem { get; set; }
        public int SearchRange { get; set; }
        public double SecurityReq { get; set; }
        public int PageNumber { get; set; }
        public int PricePercentage { get; set; }
        public int NumberPerPage { get; set; }

        public async Task<bool> GetOrders(string regionID)
        {
            HttpClient client = new HttpClient();
            string path = "https://esi.evetech.net/latest/markets/" + regionID + "/orders/?datasource=tranquility&order_type=" + OrderType + "&page=1&type_id=" + TypeID;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                if (response != null)
                {
                    AddRange(JsonConvert.DeserializeObject<List<MarketOrder>>(await response.Content.ReadAsStringAsync()));
                    //foreach (MarketOrder order in this)
                    //{
                    //    Console.WriteLine("Sys: " + FindSystemName(order.system_id) + "  Price: " + order.price);
                    //}
                    return true;
                }
                else
                {
                    Console.WriteLine("Error: GetOrders deserialize failed");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("GetOrders Response error: " + response.StatusCode);
                return false;
            }
        }

        public async Task<bool> GetAllOrders(List<string> regionsInRange)
        {
            foreach (string regionID in regionsInRange)
            {
                await GetOrders(regionID);
            }
            return true;
        }

        public async Task<List<string>> GetRoute(string systemID)
        {
            HttpResponseMessage response = await GetRouteContent(OriginSystem, systemID);
            if (response != null)
            {
                return await DeserializeRouteResponse(response);
            }
            else
            {
                Console.WriteLine("get route failed");
                return new List<string>();
            }
        }

        public int FilterTopPrice()
        {
            double max = this.Max(p => p.price);
            double min = this.Min(p => p.price);
            double selected_range = max - ((max - min) / (100 / PricePercentage));
            int numFiltered = 0;
            ForEach(delegate (MarketOrder order)
            {
                if (order.price < selected_range)
                {
                    Remove(order);
                    numFiltered++;
                }
            });
            return numFiltered;
        }

        public int FilterTopNumber(string type)
        {
            int listSize = Count;
            List<MarketOrder> tempList = new List<MarketOrder>();
            tempList.AddRange(this);
            this.Clear();
            if (type == "sell")
            {
                if (listSize >= NumberPerPage)
                {
                    AddRange(tempList.OrderBy(p => p.price).ToList().GetRange(0, NumberPerPage));
                    return listSize - NumberPerPage;
                }
                else
                {
                    AddRange(tempList.OrderBy(p => p.price).ToList());
                    return listSize;
                }
            }
            else
            {
                if (listSize >= NumberPerPage)
                {
                    AddRange(tempList.OrderByDescending(p => p.price).ToList().GetRange(0, NumberPerPage));
                    return listSize - NumberPerPage;
                }
                else
                {
                    AddRange(tempList.OrderByDescending(p => p.price).ToList());
                    return listSize;
                }
            }    
        }

        public int FilterBySecurity()
        {
            AssetLUT assets = new AssetLUT();
            int numFiltered = 0;
            List<MarketOrder> tempList = new List<MarketOrder>();
            tempList.AddRange(this);
            ForEach(delegate (MarketOrder order)
            {
                if (assets.GetSecurity(order.system_id) < SecurityReq)
                {
                    tempList.Remove(order);
                    numFiltered++;
                }
            });
            this.Clear();
            AddRange(tempList);
            return numFiltered;
        }
    }
}