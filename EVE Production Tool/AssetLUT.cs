using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVE_Production_Tool
{
    class AssetLUT
    {
        public bool CheckItemName(string name)
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

        public string GetItemID(string name)
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

        public string GetItemName(string id)
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

        public string FindRegionName(string regionID)
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

        public string FindRegionID(string regionName)
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

        public List<string> GetAllRegionNames()
        {
            List<string> names = new List<string>();
            string[] entries = System.IO.File.ReadAllLines(@"RegionLUTfile.txt");
            foreach (string line in entries)
            {
                names.Add(line.Substring(9));
            }
            return names;
        }

        public List<string> GetAllRegionIDs()
        {
            List<string> IDs = new List<string>();
            string[] entries = System.IO.File.ReadAllLines(@"RegionLUTfile.txt");
            foreach (string line in entries)
            {
                IDs.Add(line.Substring(0, 8));
            }
            return IDs;
        }

        public string FindSystemName(string systemID)
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

        public string FindSystemID(string systemName)
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

        public List<string> GetSystemsInRegion(string regionID)
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

        public string GetRegionOfSystem(string systemID)
        {
            string[] lines = System.IO.File.ReadAllLines(@"RegionSystemLUTfile.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (line.Contains(systemID))
                {
                    return parts[0];
                }
            }
            return null;
        }

        public double GetSecurity(string systemID)
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