
using System.Collections.Specialized;
using System.Drawing;
using Adventure;
using Utils;

namespace DEBUFF
{
    public class Debuff
    {

        public static List<string> Debuffs = new List<string>() { "cleanse", "poison", "cold", "hot", "lmao" };
        public static string currentPlayerDebuff = "No debuff... yet...";
        private static int currentDebuffDamage = 0;
        public static int currentDebuffDuration = 0;
        public static bool playerDebuff = true;
        public static string preferredColor = String.Empty;
        public static int DebuffTick()
        {

            if (currentDebuffDuration > 0)
            {
                currentDebuffDuration--;
                return currentDebuffDamage;
            }
            playerDebuff = false;
            currentPlayerDebuff = "No debuff... yet...";
            return 0;

        }
        public static bool CheckDebuff(string debuffValue)
        {
            foreach (string element in Debuffs)
            {
                if (debuffValue == element)
                {
                    if(debuffValue == "cleanse") 
                        SetDebuffAction("No debuff... yet...", 0, 0);
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
                return DebuffCold();
            }
            else if (debuffType == "lmao")
            {
                return 500;
            }
            return 0;
        }

        private static int DebuffPoison()
        {
            preferredColor = ANSICodes.Colors.Green;
            SetDebuffAction("poisoned", 3, 1);
            return currentDebuffDamage;
        }

        private static int DebuffCold()
        {
            preferredColor = ANSICodes.Colors.Blue;
            SetDebuffAction("cold", 5, 1);
            return currentDebuffDamage;
        }

        private static void SetDebuffAction(string currentDebuff, int debuffDuration, int debuffDamage)
        {
            playerDebuff = true;
            currentPlayerDebuff = currentDebuff;
            currentDebuffDuration = debuffDuration;
            currentDebuffDamage = debuffDamage;
        }

    }
}


