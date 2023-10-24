using DEBUFF;
using Adventure.BuildingBlocks;

namespace Adventure
{


    public class Player 
    {

        public int hp { get; set; }

        public Dictionary<String, Item> Inventory { get; set; }
        public bool cold { get; set; }

        public Player()
        {
            hp = 3;
            cold = false;
        }

    }


}