using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace EVE_Production_Tool
{
    class OrderList_Panel : TableLayoutPanel
    {
        public List<string> DataList { get; set; }

        public void Build()
        {
            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColumnCount; col++)
                {
                    Label dataLabel = new Label()
                    {
                        Font = new Font("Arial", 8),
                        Text = DataList[(row * ColumnCount) + col]
                    };
                    //Console.WriteLine(dataLabel.Text + "  c: "+ col + "  r: " + row);
                    Controls.Add(dataLabel, col, row);
                }
            }
        }
    }
}
