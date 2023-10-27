using Microsoft.VisualBasic;

namespace Adventure.BuildingBlocks
{
    public class Item
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string DescriptionType { get; set; }
        public string Status { get; set; }
        public string Damage { get; set; }
        public string Type { get; set; }


        public Dictionary<string, string[]> actions { get; set; }

        public Item()
        {
            actions = new Dictionary<string, string[]>();
        }



    }



}