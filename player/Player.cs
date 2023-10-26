using DEBUFF;
using Adventure.BuildingBlocks;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Diagnostics.Tracing;
using Utils;
using System.Threading.Tasks.Dataflow;

namespace Adventure
{

    public class Player
    {

        public int hp { get; set; }
        public int damage { get; set; }

        //public Dictionary<String, Item> Inventory { get; set; }
        public List<PlayerItem> inventory = new List<PlayerItem>();
        //public Dictionary<string, Item> Inventory = new Dictionary<String, Item>();
        public bool cold { get; set; }


        public Player()
        {
            //Inventory.Add("lol", "ds");
            hp = 10;
            damage = 5;
        }

        public void InventoryAdd(string id, string damage, string desc)
        {
            //Inventory.Add(name, locationItem);
            inventory.Add(new PlayerItem()
            {
                ItemID = id,
                ItemDmg = damage,
                ItemDesc = desc
            });

        }

        public void InventoryRemove(string removeID)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].ItemID.Equals(removeID))
                {
                    inventory.RemoveAt(i);
                }
            }
        }

        public void InventoryDisplay()
        {
            /*

               Shows all item names
               Can cycle through displayed items and pick one
               Picked item shows dmg, description etc

            */

            if(inventory.Count == 0) {
                Output.Write("Inventory empty lmao");
            }

            for (int i = 0; i < inventory.Count; i++) 
            {
                Output.Write("Name: " + inventory[i].ItemID, true);
                Output.Write("Damage" + inventory[i].ItemDmg, true);
                Output.Write("Description" + inventory[i].ItemDesc, true);
            }
        }


    }

    public class PlayerItem
    {
        public string ItemID { get; set; }
        public string ItemDmg { get; set; }
        public string ItemDesc { get; set; }
    }


}