using DEBUFF;
using Adventure.BuildingBlocks;

namespace Adventure
{


    public class Player 
    {

        public int hp { get; set; }
        public int damage { get; set; }

        //public Dictionary<String, Item> Inventory { get; set; }
        public Dictionary<string, string> Inventory = new Dictionary<string, string>();
        public bool cold { get; set; }

        public Player()
        {
            Inventory.Add("lol", "ds");
            hp = 10;
            damage = 5;
        }


    }


}