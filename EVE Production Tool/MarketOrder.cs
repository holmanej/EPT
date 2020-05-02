using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVE_Production_Tool
{
    class MarketOrder
    {
        public int duration { get; set; }
        public bool is_buy_order { get; set; }
        public DateTime issued { get; set; }
        public string location_id { get; set; }
        public int min_volume { get; set; }
        public string order_id { get; set; }
        public double price { get; set; }
        public string range { get; set; }
        public string system_id { get; set; }
        public string type_id { get; set; }
        public int volume_remain { get; set; }
        public int volume_total { get; set; }
    }
}
