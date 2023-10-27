using System.ComponentModel.Design;
using System.Reflection.Metadata;
using Microsoft.VisualBasic;
using Utils;

namespace Adventure.BuildingBlocks
{
    public class Location
    {

        public string Id { get; set; }
        public string Description { get; set; }
        public string DescriptionType { get; set; }
        public string Damage { get; set; }
        public List<string> LocationInventoryIDs { get; set; }
        public List<string> LocationDescriptionType { get; set; }
        public Dictionary<String, Item> Inventory { get; set; }
        public HashSet<String> keywords { get; set; } // HasSet is a collection that only allowes unique entries (so a word cant be in the collection twice)

        public Location()
        {
            Id = string.Empty;
            Description = string.Empty;
            DescriptionType = string.Empty;
            Damage = string.Empty;
            Inventory = new Dictionary<string, Item>();
            LocationInventoryIDs = new List<string>();
            LocationDescriptionType = new List<string>();
        }


    }


}