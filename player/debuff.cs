
using Adventure;

namespace DEBUFF
{
    public class Debuff
    {

        public static List<string> Debuffs = new List<string>() { "poison", "cold", "hot", "lmao" };
        private static string currentDebuffPlayer = string.Empty;
        public static int CurrentDebuffDuration = 0;
        public static int DebuffTick(int hero) {
            if(CurrentDebuffDuration > 0)
                return CurrentDebuffDuration--;
            return 0;
        }
        public static bool CheckDebuff(string debuffValue)
        {
            foreach (string element in Debuffs)
            {
                if (debuffValue == element)
                {
                    return true;
                }
            }
            return false;
        }
        public static int ApplyDebuff(string debuffType)
        {
            if (debuffType == "poison")
            {
                return DebuffPoison();
            }
            else if (debuffType == "cold")
            {
                return -1000;
            }
            else if (debuffType == "lmao")
            {
                return 500;
            }
            return 0;
        }

        private static int DebuffPoison() {
            CurrentDebuffDuration = 2;
            int damage = -100;
            return damage;
        }

    }
}


