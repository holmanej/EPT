using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EVE_Production_Tool.AssetLUT;

namespace EVE_Production_Tool
{
    static class BluePrintManager
    {
        public struct T1_BP
        {
            public string Item_Name { get; set; }
            public Dictionary<string, int> Mats { get; set; }
        }

        public static List<T1_BP> GetAllT1BPs(List<BluePrint> blueprints)
        {
            List<BluePrint> bps = new List<BluePrint>();
            bps.AddRange(blueprints);
            string[] specialBPs = System.IO.File.ReadAllLines(@"SpecialBPList.txt");
            foreach (BluePrint bp in blueprints)
            {
                int.TryParse(bp.materialID, out int matID);
                int.TryParse(bp.activityID, out int actID);
                
                if (matID < 34 || matID > 40 || actID != 1 || specialBPs.Contains(bp.typeID))
                {
                    bps.RemoveAll(t => t.typeID == bp.typeID);
                }
            }

            List<T1_BP> T1BPs = new List<T1_BP>();
            foreach (BluePrint bp in bps)
            {
                //Console.WriteLine("read: " + bp.typeID);
                if (!T1BPs.Exists(n => n.Item_Name == bp.typeID))
                {
                    //Console.WriteLine("creating: " + bp.typeID);
                    T1_BP tempBP = new T1_BP
                    {
                        Item_Name = bp.typeID
                    };
                    Dictionary<string, int> tempMats = new Dictionary<string, int>();
                    foreach (BluePrint matvalue in bps.FindAll(n => n.typeID == bp.typeID))
                    {
                        //Console.WriteLine("adding mat: " + matvalue.typeID + " - " + matvalue.materialID);
                        int.TryParse(matvalue.quantity, out int q);
                        tempMats.Add(matvalue.materialID, q);
                    }
                    tempBP.Mats = tempMats;
                    T1BPs.Add(tempBP);
                }
            }
            return T1BPs;
        }
    }
}
