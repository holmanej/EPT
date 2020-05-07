using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EVE_Production_Tool
{
    class MarketBrowser : Panel
    {
        private TableLayoutPanel ResultsHolder = new TableLayoutPanel();
        private Panel SearchPanel = new Panel();
        private AssetLUT Assets = new AssetLUT();

        public MarketBrowser()
        {
            Size = new Size(800, 670);

            Label searchBox_Label = new Label()
            {
                Location = new Point(20, 0),
                Size = new Size(100, 15),
                Text = "Item Name"
            };
            SearchPanel.Controls.Add(searchBox_Label);
            TextBox searchBox = new TextBox()
            {
                Location = new Point(20, 25),
                Size = new Size(140, 20)
            };
            searchBox.KeyPress += SearchBox_KeyPress;
            SearchPanel.Controls.Add(searchBox);

            Label buyOrSell = new Label()
            {
                Location = new Point(175, 25),
                Size = new Size(40, 20),
                Text = "sell",
                BackColor = Color.DarkGray,
                TextAlign = ContentAlignment.MiddleCenter
            };
            buyOrSell.Click += BuyOrSell_Click;
            SearchPanel.Controls.Add(buyOrSell);

            Label originBox_Label = new Label()
            {
                Location = new Point(20, 55),
                Size = new Size(100, 15),
                Text = "Current System"
            };
            SearchPanel.Controls.Add(originBox_Label);
            TextBox originBox = new TextBox()
            {
                Location = new Point(20, 75),
                Size = new Size(140, 20)
            };
            SearchPanel.Controls.Add(originBox);

            Label proxBox_Label = new Label()
            {
                Location = new Point(175, 55),
                Size = new Size(100, 15),
                Text = "Range"
            };
            SearchPanel.Controls.Add(proxBox_Label);
            TextBox proxBox = new TextBox()
            {
                Location = new Point(175, 75),
                Size = new Size(40, 20)
            };
            SearchPanel.Controls.Add(proxBox);

            Label numOrders_Label = new Label()
            {
                Location = new Point(20, 100),
                Size = new Size(100, 15),
                Text = "# Orders"
            };
            SearchPanel.Controls.Add(numOrders_Label);
            TextBox numOrders = new TextBox()
            {
                Location = new Point(20, 120),
                Size = new Size(40, 20)
            };
            SearchPanel.Controls.Add(numOrders);

            Label safeOrNah = new Label()
            {
                Location = new Point(70, 120),
                Size = new Size(45, 20),
                BackColor = Color.DarkGray,
                Text = "safe",
                TextAlign = ContentAlignment.MiddleCenter
            };
            safeOrNah.Click += SafeOrNah_Click;
            SearchPanel.Controls.Add(safeOrNah);

            Button searchGo = new Button()
            {
                Location = new Point(125, 120),
                Size = new Size(90, 22),
                Text = "Search"
            };
            searchGo.Click += SearchGo_Click;
            SearchPanel.Controls.Add(searchGo);

            TextBox statusConsole = new TextBox()
            {
                Multiline = true,
                ReadOnly = true,
                Location = new Point(20, 170),
                Size = new Size(200, 300),
                Name = "statusConsole"
            };
            SearchPanel.Controls.Add(statusConsole);

            SearchPanel.Location = new Point(20, 20);
            SearchPanel.Size = new Size(220, 550);
            Controls.Add(SearchPanel);
            Controls.Add(ResultsHolder);
        }

        private void SearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox thisBox = (TextBox)sender;
            string itemName = thisBox.Text;
            if (itemName == "")
            {
                thisBox.BackColor = Color.White;
            }
            else if (Assets.CheckItemName(itemName))
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
        //    //await GetPopularT2ItemsAsync();

        //    MarketOrder_List ordersList = new MarketOrder_List()
        //    {
        //        OrderType = "sell",
        //        OriginSystem = FindSystemID("Frarn"),
        //        NumberPerPage = 5
        //    };

        //    Dictionary<string, double> matPrices = new Dictionary<string, double>();
        //    for (int i = 34; i < 41; i++)
        //    {
        //        ordersList.TypeID = i.ToString();
        //        await ordersList.GetOrders(FindRegionID("The Forge"));
        //        ordersList.FilterTopNumber("buy");
        //        double averagePrice = ordersList.Sum(p => p.price) / ordersList.Count;
        //        //Console.WriteLine("avPrice of " + ordersList.TypeID + " : " + averagePrice.ToString());
        //        matPrices.Add(ordersList.TypeID, averagePrice);
        //        ordersList.Clear();
        //    }

        //    List<KeyValuePair<string, double>> BP_Profits = new List<KeyValuePair<string, double>>();
        //    int progress = 0;
        //    List<T1_BP> bps = GetAllT1BPs(ReadBluePrintFile());
        //    foreach (T1_BP bp in bps)
        //    {
        //        ordersList.TypeID = bp.Item_Name;
        //        ordersList.Clear();
        //        double bpCost = 0;
        //        foreach (string mat in bp.Mats.Keys)
        //        {
        //            bp.Mats.TryGetValue(mat, out int quantity);
        //            matPrices.TryGetValue(mat, out double price);
        //            bpCost += quantity * price;
        //        }
        //        string itemName = GetItemName(bp.Item_Name);
        //        if (itemName.Contains("Blueprint"))
        //        {
        //            itemName = itemName.Remove(itemName.IndexOf("Blueprint") - 1, 10);
        //        }
        //        string itemID = GetItemID(itemName);
        //        ordersList.TypeID = itemID;
        //        await ordersList.GetOrders(FindRegionID("The Forge"));
        //        ordersList.FilterTopNumber("sell");
        //        double averagePrice;
        //        if (ordersList.Count == 0)
        //        {
        //            averagePrice = 0;
        //        }
        //        else
        //        {
        //            averagePrice = ordersList.Sum(p => p.price) / ordersList.Count;
        //        }
        //        itemName = GetItemName(itemID);
        //        Console.WriteLine(progress++ + "/" + bps.Count + "  Item: " + itemName + "  Cost: " + bpCost + "  Rev: " + averagePrice + "  Profit: " + (averagePrice - bpCost).ToString());
        //        BP_Profits.Add(new KeyValuePair<string, double>(itemName, averagePrice / bpCost));
        //        ordersList.Clear();
        //        //Console.WriteLine(progress++ + "/" + bps.Count);
            //}

            //BP_Profits = BP_Profits.OrderByDescending(p => p.Value).ToList();
            //Console.WriteLine("Results");
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine("ID: " + BP_Profits[i].Key + "  Profit: " + BP_Profits[i].Value);
            //}
        }

        private async void SearchGo_Click(object sender, EventArgs e)
        {
            ControlCollection searchData = SearchPanel.Controls;
            SearchPanel.Controls.Find("statusConsole", true)[0].ResetText();
            ResultsHolder.Controls.Clear();

            List<string> inputs = new List<string>();
            int controlNum = 0;
            foreach (Control input in searchData)
            {
                inputs.Add(input.Text);
                Console.WriteLine(controlNum++ + ": " + input.Text);
            }
            MarketOrder_List ordersList = new MarketOrder_List()
            {
                TypeID = Assets.GetItemID(inputs[1]),
                OrderType = inputs[2],
                OriginSystem = Assets.FindSystemID(inputs[4]),
                SearchRange = int.Parse(inputs[6]),
                NumberPerPage = int.Parse(inputs[8]),
                SecurityReq = inputs[9] == "safe" ? 0.5 : 0,
                PageNumber = 0,
                PricePercentage = 20
            };
            TextBox readOut = (TextBox)searchData[searchData.Count - 1];

            RouteFinder router = new RouteFinder("SystemJumpsLUTfile.txt", ordersList.OriginSystem, ordersList.SearchRange);

            Console.WriteLine("Getting regions in range");
            readOut.AppendText("Getting regions in range\r\n");
            List<string> systems = router.GetSystemsInRange(ordersList.SearchRange);
            List<string> regionsInRange = new List<string>();
            AssetLUT assets = new AssetLUT();
            foreach (string sys in systems)
            {
                string region = assets.GetRegionOfSystem(sys);
                if (!regionsInRange.Exists(r => r == region))
                {
                    regionsInRange.Add(region);
                }
            }

            Console.WriteLine("Getting orders. Regions: " + regionsInRange.Count);
            readOut.AppendText("Getting orders. Regions: " + regionsInRange.Count + "\r\n");
            await ordersList.GetAllOrders(regionsInRange);
            MarketOrder_List temp = new MarketOrder_List();
            foreach (MarketOrder order in ordersList)
            {
                if (router.Paths.Exists(n => order.system_id == n.ID))
                {
                    temp.Add(order);
                }
            }
            ordersList.Clear();
            ordersList.AddRange(temp);

            Console.WriteLine("Filtering orders. Count: " + ordersList.Count);
            readOut.AppendText("Filtering orders. Count: " + ordersList.Count + "\r\n");

            int numFiltered = ordersList.FilterBySecurity();
            Console.WriteLine("Filtered by security: " + numFiltered + "\r\n");
            readOut.AppendText("Filtered by security: " + numFiltered + "\r\n");

            numFiltered = ordersList.FilterTopNumber();
            Console.WriteLine("Filtered by number: " + numFiltered);
            readOut.AppendText("Filtered by number: " + numFiltered + "\r\n");

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
                List<string> route = router.GetRoute(order.system_id);
                int distance = route.Count - 1;
                Console.WriteLine(progress + "/" + ordersList.Count);
                readOut.Text = readOut.Text.Replace(progress + "/" + ordersList.Count + "\r\n", (progress + 1) + "/" + ordersList.Count + "\r\n");
                progress++;
                orderData.Add(Assets.FindSystemName(order.system_id));
                orderData.Add(distance.ToString());
                orderData.Add(order.price.ToString());
                orderData.Add(order.volume_remain.ToString());
                orderData.Add(Assets.FindSystemName(route[distance]));
            }

            ResultsHolder.Location = new Point(250, 20);
            ResultsHolder.Size = new Size(800, 600);
            ResultsHolder.ColumnCount = 5;
            ResultsHolder.RowCount = ordersList.Count + 1;
            for (int row = 0; row < ResultsHolder.RowCount; row++)
            {
                for (int col = 0; col < ResultsHolder.ColumnCount; col++)
                {
                    Label dataLabel = new Label()
                    {
                        Font = new Font("Arial", 8),
                        Text = orderData[(row * ResultsHolder.ColumnCount) + col]
                    };
                    //Console.WriteLine(dataLabel.Text + "  c: "+ col + "  r: " + row);
                    ResultsHolder.Controls.Add(dataLabel, col, row);
                }
            }
            Console.WriteLine("Done");
            readOut.AppendText("Done\r\n");
        }

    }
}
