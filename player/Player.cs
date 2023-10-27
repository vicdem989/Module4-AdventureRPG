using DEBUFF;
using Adventure.BuildingBlocks;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Diagnostics.Tracing;
using Utils;
using System.Threading.Tasks.Dataflow;
using System.Net;

namespace Adventure
{

    public class Player
    {
        #region // Variable declation //
        public int hp { get; set; }
        public int damage { get; set; }
        public bool inventoryOpen = false;

        private const string MAINSTORY = "mainstory";



        public List<PlayerItem> inventory = new List<PlayerItem>();
        public List<string> playerCommands = new List<string>();

        #endregion
        public Player()
        {
            hp = 10;
            damage = 5;
        }

        public void InventoryAdd(string id, string damage, string desc, string status, string itemtype = "trash")
        {
            inventory.Add(new PlayerItem()
            {
                ItemID = id.ToLower(),
                ItemDmg = damage,
                ItemDesc = desc,
                DefaultItemStatus = status,
                ItemStatus = status,
                ItemType = itemtype
            });

        }

        public string InventoryRemove(string itemToRemove)
        {
            string outputItemID = string.Empty;
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].ItemID.Contains(itemToRemove) && inventory[i].ItemType != MAINSTORY)
                {
                    outputItemID = inventory[i].ItemID;
                    inventory.RemoveAt(i);
                    return "item deleted => " + outputItemID;
                }
                return "Ye u kinda need that item later sorta... no spoilers tho...";
            }
            return "unlucky?";
        }

        public string InventoryDisplay()
        {
            /*

               Shows all item names
               Can cycle through displayed items and pick one
               Picked item shows dmg, description etc

            */

            if (inventory.Count <= 0)
            {
                return ("Inventory empty lmao");
            }
            inventoryOpen = true;
            string output = "Weow inventory is empty no more: \n";
            for (int i = 0; i < inventory.Count; i++)
            {
                output += inventory[i].ItemID + ", \n";
            }
            return output;
        }

        public bool VerifyItemInInventory(string itemToRemove)
        {
            foreach (PlayerItem item in inventory)
            {
                if (item.ItemID == itemToRemove)
                {
                    return true;
                }
            }
            return false;

        }


        public class PlayerItem
        {
            public string ItemID { get; set; }
            public string ItemDmg { get; set; }
            public string ItemDesc { get; set; }
            public string DefaultItemStatus { get; set; }
            public string ItemStatus { get; set; }
            public string ItemType { get; set; }
        }
    }
}