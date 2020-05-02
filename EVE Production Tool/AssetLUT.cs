using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EVE_Production_Tool
{
    static class AssetLUT
    {
        public struct BluePrint
        {
            public string typeID { get; set; }
            public string activityID { get; set; }
            public string materialID { get; set; }
            public string quantity { get; set; }
        }

        public static bool CheckItemName(string name)
        {
            string[] entries = System.IO.File.ReadAllLines(@"ItemLUTfile.txt");
            foreach (string line in entries)
            {
                string[] parts = line.Split(',');
                if (parts[1] == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetItemID(string name)
        {
            string[] entries = System.IO.File.ReadAllLines(@"ItemLUTfile.txt");
            foreach (string line in entries)
            {
                if (line.Contains(name))
                {
                    string[] parts = line.Split(',');
                    return parts[0];
                }
            }
            return null;
        }

        public static string GetItemName(string id)
        {
            string[] entries = System.IO.File.ReadAllLines(@"ItemLUTfile.txt");
            foreach (string line in entries)
            {
                if (line.Contains(id))
                {
                    string[] parts = line.Split(',');
                    return parts[1];
                }
            }
            return null;
        }

        public static List<BluePrint> ReadBluePrintFile()
        {
            string[] entries = System.IO.File.ReadAllLines(@"BPLUTfile.txt");
            List<BluePrint> BPs = new List<BluePrint>();
            foreach (string line in entries)
            {
                string[] fields = line.Split(',');
                //Console.WriteLine(fields[0]);
                //Console.WriteLine(fields[1]);
                //Console.WriteLine(fields[2]);
                //Console.WriteLine(fields[3]);
                BPs.Add(new BluePrint()
                {
                    typeID = fields[0],
                    activityID = fields[1],
                    materialID = fields[2],
                    quantity = fields[3]
                });
            }
            return BPs;
        }

        public static string FindRegionName(string regionID)
        {
            string[] entries = System.IO.File.ReadAllLines(@"RegionLUTfile.txt");
            foreach (string line in entries)
            {
                if (line.Contains(regionID))
                {
                    return line.Substring(9);
                }
            }
            return "region not found";
        }

        public static string FindRegionID(string regionName)
        {
            string[] entries = System.IO.File.ReadAllLines(@"RegionLUTfile.txt");
            foreach (string line in entries)
            {
                if (line.Contains(regionName))
                {
                    return line.Substring(0, 8);
                }
            }
            return "system not found";
        }

        public static List<string> GetAllRegionNames()
        {
            List<string> names = new List<string>();
            string[] entries = System.IO.File.ReadAllLines(@"RegionLUTfile.txt");
            foreach (string line in entries)
            {
                names.Add(line.Substring(9));
            }
            return names;
        }

        public static List<string> GetAllRegionIDs()
        {
            List<string> IDs = new List<string>();
            string[] entries = System.IO.File.ReadAllLines(@"RegionLUTfile.txt");
            foreach (string line in entries)
            {
                IDs.Add(line.Substring(0, 8));
            }
            return IDs;
        }

        public static string FindSystemName(string systemID)
        {
            string[] entries = System.IO.File.ReadAllLines(@"SystemLUTfile.txt");
            foreach (string line in entries)
            {
                if (line.Contains(systemID))
                {
                    return line.Substring(9);
                }
            }
            return "system not found";
        }

        public static string FindSystemID(string systemName)
        {
            string[] entries = System.IO.File.ReadAllLines(@"SystemLUTfile.txt");
            foreach (string line in entries)
            {
                if (line.Contains(systemName))
                {
                    return line.Substring(0, 8);
                }
            }
            return "system not found";
        }

        public static List<string> GetSystemsInRegion(string regionID)
        {
            string[] lines = System.IO.File.ReadAllLines(@"RegionSystemLUTfile.txt");
            List<string> systems = new List<string>();
            foreach (string line in lines)
            {
                if (line.Contains(regionID))
                {
                    systems.Add(line.Substring(9));
                }
            }
            return systems;            
        }

        public static double GetSecurity(string systemID)
        {
            string[] lines = System.IO.File.ReadAllLines(@"SystemSecurityLUTfile.txt");
            foreach (string line in lines)
            {
                if (line.Contains(systemID))
                {
                    //Console.WriteLine(line);
                    if (double.TryParse(line.Substring(9), out double security))
                    {
                        if (security < 0)
                        {
                            return 0;
                        }
                        else
                        {
                            return Math.Round(security);
                        }
                    }
                }
            }
            return -1;
        }
    }
}