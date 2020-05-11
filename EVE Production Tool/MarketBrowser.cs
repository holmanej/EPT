using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private MarketCache OrderCache = new MarketCache();

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
                Size = new Size(140, 20),
                Text = "Tritanium"
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
                Size = new Size(140, 20),
                Text = "Jita"
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
                Size = new Size(40, 20),
                Text = "100"
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
                Size = new Size(40, 20),
                Text = "25"
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

        private void SearchGo_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
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

            RouteFinder router = new RouteFinder(ordersList.OriginSystem);
            
            List<MarketOrder> temp = OrderCache.Orders;
            temp.RemoveAll(o => o == null);

            Console.WriteLine("Filtering orders. Count: " + temp.Count);
            readOut.AppendText("Filtering orders. Count: " + temp.Count + "\r\n");

            int numFiltered = temp.RemoveAll(o => o.type_id != ordersList.TypeID.ToString());
            Debug.WriteLine("Filtered by item: " + numFiltered);
            readOut.AppendText("Filtered by item: " + numFiltered + "\r\n");

            numFiltered = temp.RemoveAll(o => o.is_buy_order == (ordersList.OrderType == "sell"));
            Debug.WriteLine("Filtered by type: " + numFiltered);
            readOut.AppendText("Filtered by type: " + numFiltered + "\r\n");

            List<int> systems = router.GetSystemsInRange(ordersList.SearchRange);
            temp.ForEach(delegate (MarketOrder order)
            {
                if (systems.Exists(s => s == int.Parse(order.system_id)))
                {
                    ordersList.Add(order);
                }
            });

            //temp.ForEach(delegate (MarketOrder order)
            //{
            //    int dist = router.GetDistance(int.Parse(order.system_id));
            //    if (dist <= ordersList.SearchRange && dist >= 0)
            //    {
            //        ordersList.Add(order);
            //    }
            //});
            numFiltered = temp.Count - ordersList.Count;
            Debug.WriteLine("Filtered by distance: " + numFiltered);
            readOut.AppendText("Filtered by distance: " + numFiltered + "\r\n");

            numFiltered = ordersList.FilterBySecurity();
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
                "Quantity"
            };

            foreach (MarketOrder order in ordersList)
            {
                List<int> route = router.GetRoute(int.Parse(order.system_id));
                int distance = route.Count - 1;
                Console.WriteLine(progress + "/" + ordersList.Count);
                readOut.Text = readOut.Text.Replace(progress + "/" + ordersList.Count + "\r\n", (progress + 1) + "/" + ordersList.Count + "\r\n");
                progress++;
                orderData.Add(Assets.FindSystemName(int.Parse(order.system_id)));
                orderData.Add(distance.ToString());
                orderData.Add(order.price.ToString());
                orderData.Add(order.volume_remain.ToString());
            }

            ResultsHolder.Location = new Point(250, 20);
            ResultsHolder.Size = new Size(800, 600);
            ResultsHolder.ColumnCount = 4;
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
            stopwatch.Stop();
            Debug.WriteLine("timed: " + stopwatch.Elapsed);
        }

    }
}
