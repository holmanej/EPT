using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EVE_Production_Tool.ESI;
using static EVE_Production_Tool.AssetLUT;
using static EVE_Production_Tool.BluePrintManager;
using System.Net.Http;

namespace EVE_Production_Tool
{
    public partial class Form1 : Form {

        public TableLayoutPanel resultsHolder = new TableLayoutPanel();
        public Panel searchPanel = new Panel();

        public Form1() {
            InitializeComponent();

            Size = new Size(800, 670);

            SetupMarketBrowser();
        }

        private void SetupMarketBrowser()
        {
            Label searchBox_Label = new Label()
            {
                Location = new Point(20, 0),
                Size = new Size(100, 15),
                Text = "Item Name"
            };
            searchPanel.Controls.Add(searchBox_Label);
            TextBox searchBox = new TextBox()
            {
                Location = new Point(20, 25),
                Size = new Size(140, 20)
            };
            searchBox.KeyPress += SearchBox_KeyPress;
            searchPanel.Controls.Add(searchBox);

            Label buyOrSell = new Label()
            {
                Location = new Point(175, 25),
                Size = new Size(40, 20),
                Text = "sell",
                BackColor = Color.DarkGray,
                TextAlign = ContentAlignment.MiddleCenter
            };
            buyOrSell.Click += BuyOrSell_Click;
            searchPanel.Controls.Add(buyOrSell);

            Label originBox_Label = new Label()
            {
                Location = new Point(20, 55),
                Size = new Size(100, 15),
                Text = "Current System"
            };
            searchPanel.Controls.Add(originBox_Label);
            TextBox originBox = new TextBox()
            {
                Location = new Point(20, 75),
                Size = new Size(140, 20)
            };
            searchPanel.Controls.Add(originBox);

            Label proxBox_Label = new Label()
            {
                Location = new Point(175, 55),
                Size = new Size(100, 15),
                Text = "Range"
            };
            searchPanel.Controls.Add(proxBox_Label);
            TextBox proxBox = new TextBox()
            {
                Location = new Point(175, 75),
                Size = new Size(40, 20)
            };
            searchPanel.Controls.Add(proxBox);

            Label numOrders_Label = new Label()
            {
                Location = new Point(20, 100),
                Size = new Size(100, 15),
                Text = "# Orders"
            };
            searchPanel.Controls.Add(numOrders_Label);
            TextBox numOrders = new TextBox()
            {
                Location = new Point(20, 120),
                Size = new Size(40, 20)
            };
            searchPanel.Controls.Add(numOrders);

            Label safeOrNah = new Label()
            {
                Location = new Point(70, 120),
                Size = new Size(45, 20),
                BackColor = Color.DarkGray,
                Text = "safe",
                TextAlign = ContentAlignment.MiddleCenter
            };
            safeOrNah.Click += SafeOrNah_Click;
            searchPanel.Controls.Add(safeOrNah);

            Button searchGo = new Button()
            {
                Location = new Point(125, 120),
                Size = new Size(90, 22),
                Text = "Search"
            };
            searchGo.Click += SearchGo_Click;
            searchPanel.Controls.Add(searchGo);

            TextBox statusConsole = new TextBox()
            {
                Multiline = true,
                ReadOnly = true,
                Location = new Point(20, 170),
                Size = new Size(200, 300),
                Name = "statusConsole"
            };
            searchPanel.Controls.Add(statusConsole);

            searchPanel.Location = new Point(20, 20);
            searchPanel.Size = new Size(220, 550);
            Controls.Add(searchPanel);
            Controls.Add(resultsHolder);
        }

        private void SearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox thisBox = (TextBox)sender;
            string itemName = thisBox.Text;
            if (itemName == "")
            {
                thisBox.BackColor = Color.White;
            }
            else if (CheckItemName(itemName))
            {
                thisBox.BackColor = Color.Red;
            }
            else
            {
                thisBox.BackColor = Color.Green;
            }
        }

        private void SafeOrNah_Click(object sender, EventArgs e)
        {
            Label thisLabel = (Label)sender;
            if (thisLabel.Text == "safe")
            {
                thisLabel.Text = "UNsafe";
            }
            else
            {
                thisLabel.Text = "safe";
            }
        }

        private void BuyOrSell_Click(object sender, EventArgs e)
        {
            Label thisLabel = (Label)sender;
            if (thisLabel.Text == "sell")
            {
                thisLabel.Text = "buy";
            }
            else
            {
                thisLabel.Text = "sell";
            }
        }

        private async void LoadBPs_Button_Click(object sender, EventArgs e)
        {
            //await GetPopularT2ItemsAsync();

            MarketOrder_List ordersList = new MarketOrder_List()
            {
                OrderType = "sell",
                OriginSystem = FindSystemID("Frarn"),
                NumberPerPage = 5
            };

            Dictionary<string, double> matPrices = new Dictionary<string, double>();
            for (int i = 34; i < 41; i++)
            {
                ordersList.TypeID = i.ToString();
                await ordersList.GetOrders(FindRegionID("The Forge"));
                ordersList.FilterTopNumber("buy");
                double averagePrice = ordersList.Sum(p => p.price) / ordersList.Count;
                //Console.WriteLine("avPrice of " + ordersList.TypeID + " : " + averagePrice.ToString());
                matPrices.Add(ordersList.TypeID, averagePrice);
                ordersList.Clear();
            }

            List<KeyValuePair<string, double>> BP_Profits = new List<KeyValuePair<string, double>>();
            int progress = 0;
            List<T1_BP> bps = GetAllT1BPs(ReadBluePrintFile());
            foreach (T1_BP bp in bps)
            {
                ordersList.TypeID = bp.Item_Name;
                ordersList.Clear();
                double bpCost = 0;
                foreach (string mat in bp.Mats.Keys)
                {
                    bp.Mats.TryGetValue(mat, out int quantity);
                    matPrices.TryGetValue(mat, out double price);
                    bpCost += quantity * price;
                }
                string itemName = GetItemName(bp.Item_Name);
                if (itemName.Contains("Blueprint"))
                {
                    itemName = itemName.Remove(itemName.IndexOf("Blueprint") - 1, 10);
                }
                string itemID = GetItemID(itemName);
                ordersList.TypeID = itemID;
                await ordersList.GetOrders(FindRegionID("The Forge"));
                ordersList.FilterTopNumber("sell");
                double averagePrice;
                if (ordersList.Count == 0)
                {
                    averagePrice = 0;
                }
                else
                {
                    averagePrice = ordersList.Sum(p => p.price) / ordersList.Count;
                }
                itemName = GetItemName(itemID);
                Console.WriteLine(progress++ + "/" + bps.Count + "  Item: " + itemName + "  Cost: " + bpCost + "  Rev: " + averagePrice + "  Profit: " + (averagePrice - bpCost).ToString());
                BP_Profits.Add(new KeyValuePair<string, double>(itemName, averagePrice / bpCost));
                ordersList.Clear();
                //Console.WriteLine(progress++ + "/" + bps.Count);
            }

            BP_Profits = BP_Profits.OrderByDescending(p => p.Value).ToList();
            Console.WriteLine("Results");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("ID: " + BP_Profits[i].Key + "  Profit: " + BP_Profits[i].Value);
            }
        }

        private async Task<List<string>> FindRegionsInRange(string origin, int range)
        {
            List<string> regionIDs = GetAllRegionIDs();
            List<string> validRegions = new List<string>();

            foreach (string regionID in regionIDs)
            {
                if (regionID.StartsWith("10"))
                {
                    List<string> systems = GetSystemsInRegion(regionID);
                    HttpResponseMessage response = await GetRouteContent(origin, systems[0]);
                    if (response != null)
                    {
                        List<string> route = await DeserializeRouteResponse(response);
                        int distance = route.Count;
                        if (distance <= range)
                        {
                            validRegions.Add(regionID);
                        }
                    }
                    else
                    {
                        //Console.WriteLine("get route failed");
                    }
                };
            }
            return validRegions;
        }

        private async void SearchGo_Click(object sender, EventArgs e)
        {
            Control.ControlCollection searchData = searchPanel.Controls;
            searchPanel.Controls.Find("statusConsole", true)[0].ResetText();
            resultsHolder.Controls.Clear();

            List<string> inputs = new List<string>();
            foreach (Control input in searchData)
            {
                inputs.Add(input.Text);
                Console.WriteLine(input.Text);
            }
            MarketOrder_List ordersList = new MarketOrder_List()
            {
                TypeID = GetItemID(inputs[1]),
                OrderType = inputs[2],
                OriginSystem = FindSystemID(inputs[4]),
                SearchRange = int.Parse(inputs[6]),
                NumberPerPage = int.Parse(inputs[8]),
                SecurityReq = inputs[9] == "safe" ? 0.5 : 0,
                PageNumber = 0,
                PricePercentage = 20
            };
            TextBox readOut = (TextBox)searchData[searchData.Count - 1];

            Console.WriteLine("Getting regions in range");
            readOut.AppendText("Getting regions in range\r\n");
            List<string> regionsInRange = await FindRegionsInRange(ordersList.OriginSystem, ordersList.SearchRange);

            Console.WriteLine("Getting orders. Regions: " + regionsInRange.Count);
            readOut.AppendText("Getting orders. Regions: " + regionsInRange.Count + "\r\n");
            await ordersList.GetAllOrders(regionsInRange);

            Console.WriteLine("Filtering orders. Count: " + ordersList.Count);
            readOut.AppendText("Filtering orders. Count: " + ordersList.Count + "\r\n");
            int numFiltered = ordersList.FilterBySecurity();
            numFiltered += ordersList.FilterTopNumber(ordersList.OrderType);
            Console.WriteLine("number filtered: " + numFiltered);
            readOut.AppendText("number filtered: " + numFiltered + "\r\n");

            Console.WriteLine("Getting route data. Count: " + ordersList.Count);
            readOut.AppendText("Getting route data. Count: " + ordersList.Count + "\r\n");
            int progress = 0;
            readOut.AppendText(progress + "/" + ordersList.Count + "\r\n");
            List<string> orderData = new List<string>
            {
                "System Name",
                "Distance",
                "Price",
                "Quantity",
                "Target System"
            };
            foreach (MarketOrder order in ordersList)
            {
                HttpResponseMessage response = await GetRouteContent(ordersList.OriginSystem, order.system_id);
                if (response != null)
                {
                    List<string> route = await DeserializeRouteResponse(response);
                    int distance = ConvertRange(order.range) > route.Count ? 0 : route.Count - ConvertRange(order.range);
                    route.Insert(0, ordersList.OriginSystem);

                    Console.WriteLine(progress + "/" + ordersList.Count);
                    readOut.Text = readOut.Text.Replace(progress + "/" + ordersList.Count + "\r\n", (progress + 1) + "/" + ordersList.Count + "\r\n");
                    progress++;
                    orderData.Add(FindSystemName(order.system_id));
                    orderData.Add(distance.ToString());
                    orderData.Add(order.price.ToString());
                    orderData.Add(order.volume_remain.ToString());
                    orderData.Add(FindSystemName(route[distance]));
                }
                else
                {
                    Console.WriteLine("get route failed");
                }
            }

            resultsHolder.Location = new Point(250, 20);
            resultsHolder.Size = new Size(800, 600);
            resultsHolder.ColumnCount = 5;
            resultsHolder.RowCount = 25;
            for (int row = 0; row < resultsHolder.RowCount; row++)
            {
                for (int col = 0; col < resultsHolder.ColumnCount; col++)
                {
                    Label dataLabel = new Label()
                    {
                        Font = new Font("Arial", 8),
                        Text = orderData[(row * resultsHolder.ColumnCount) + col]
                    };
                    //Console.WriteLine(dataLabel.Text + "  c: "+ col + "  r: " + row);
                    resultsHolder.Controls.Add(dataLabel, col, row);
                }
            }

            //OrderList_Panel ordersPanel = new OrderList_Panel
            //{
            //    Location = new Point(250, 20),
            //    Size = new Size(800, 600),
            //    ColumnCount = 5,
            //    RowCount = 20,
            //    DataList = orderData
            //};
            //ordersPanel.Build();
            Console.WriteLine("Done");
            readOut.AppendText("Done\r\n");
        }
    }
}
