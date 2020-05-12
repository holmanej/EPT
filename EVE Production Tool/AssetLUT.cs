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

        public int GetItemID(string name)
        {
            string[] entries = System.IO.File.ReadAllLines(@"ItemLUTfile.txt");
            foreach (string line in entries)
            {
                if (line.Contains(name))
                {
                    string[] parts = line.Split(',');
                    return int.Parse(parts[0]);
                }
            }
            return -1;
        }

        public int GetItemName(string id)
        {
            string[] entries = System.IO.File.ReadAllLines(@"ItemLUTfile.txt");
            foreach (string line in entries)
            {
                if (line.Contains(id))
                {
                    string[] parts = line.Split(',');
                    return int.Parse(parts[1]);
                }
            }
            return -1;
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

        public int FindRegionID(string regionName)
        {
            string[] entries = System.IO.File.ReadAllLines(@"RegionLUTfile.txt");
            foreach (string line in entries)
            {
                if (line.Contains(regionName))
                {
                    return int.Parse(line.Substring(0, 8));
                }
            }
            return -1;
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

        public string FindSystemName(int systemID)
        {
            string[] entries = System.IO.File.ReadAllLines(@"SystemLUTfile.txt");
            foreach (string line in entries)
            {
                if (line.Contains(systemID.ToString()))
                {
                    return line.Substring(9);
                }
            }
            return "system not found";
        }

        public int FindSystemID(string systemName)
        {
            string[] entries = System.IO.File.ReadAllLines(@"SystemLUTfile.txt");
            foreach (string line in entries)
            {
                if (line.Contains(systemName))
                {
                    return int.Parse(line.Substring(0, 8));
                }
            }
            return -1;
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

        public int GetRegionOfSystem(int systemID)
        {
            string[] lines = System.IO.File.ReadAllLines(@"RegionSystemLUTfile.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (line.Contains(systemID.ToString()))
                {
                    return int.Parse(parts[0]);
                }
            }
            return -1;
        }

        public double GetSecurity(string systemID)
        {
            string[] lines = System.IO.File.ReadAllLines(@"SystemSecurityLUTfile.txt");
            foreach (string line in lines)
            {
                if (line.Contains(systemID))
                {
                    return double.Parse(line.Substring(9));
                }
            }
            return -1;
        }
    }
}