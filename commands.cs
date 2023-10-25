
using Adventure;
using Adventure.BuildingBlocks;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Transactions;

namespace COMMANDS
{

    public class Commands
    {
        static Location currentLocation;
        public const string ABYSS = "game/abyss.adv";
        public static List<string> Locations = new List<string>() { "abyss" };
        public static Dictionary<string, string> LookUpLocation = new Dictionary<string, string>();
        public static string goToLocation = string.Empty;



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
            foreach (string element in Locations)
            {
                if (location == element)
                {
                    goToLocation = location;
                    return location;
                }
            }
            return "";
        }

        public static Adventure.BuildingBlocks.Location GoToLocation()
        {
            /*foreach (KeyValuePair<string, string> combi in LookUpLocation)
            {
                if()
            }*/
            currentLocation = AdvenureGame.parser.CreateLocationFromDescription(ABYSS);//goToLocation);
            return currentLocation;

        }


    }

}