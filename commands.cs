
using Adventure;
using Adventure.BuildingBlocks;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Transactions;
using OUTPUTVALUES;

namespace COMMANDS
{

    public class Commands
    {
        static Location currentLocation;
        public const string ABYSS = "game/abyss.adv";
        public const string START = "game/start.adv";
        public static List<string> ValidCommands = new List<string>() { "abyss", "start", "cleanse" };

        public static Dictionary<string, string> LookUpLocation = new Dictionary<string, string>();
        public static string goToLocation = string.Empty;

        public Commands()
        {
            LookUpLocation.Add("abyss", ABYSS);

            //GET DICTIONARY TO WORK
        }


        public static string VerifyLocation(string location)
        {
            /*foreach (KeyValuePair<string, string> element in LookUpLocation)
            {
                if (element.Key == location)
                {
                    goToLocation = element.Value;
                    return location;
                }
            }*/
            foreach (string element in ValidCommands)
            {
                if (location == element)
                {
                    //goToLocation = location;
                    if (goToLocation == "abyss")
                    {
                        goToLocation = ABYSS;
                    }

                    return location;
                }
            }
            return "";
        }

        public static Adventure.BuildingBlocks.Location GoToLocation()
        {
            currentLocation = AdvenureGame.parser.CreateLocationFromDescription(goToLocation);
            return currentLocation;

        }

        public static string godModeCheat(string command, Player Hero)
        {

            Hero.hp += 999;
            command = string.Empty;
            return "That did not work, or did it?";

        }

    }

}