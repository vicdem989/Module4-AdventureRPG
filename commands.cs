
using Adventure;
using Adventure.BuildingBlocks;
using System.Net;
using System.Transactions;

namespace COMMANDS
{

    public class Commands
    {
        static Location currentLocation;
        public const string ABYSS = "game/abyss.adv";
        public static List<string> Locations = new List<string>() {"abyss"};
        public static string goToLocation = string.Empty;

        public static string VerifyLocation(string location) {
            foreach (string element in Locations) {
                if(location == element) {
                    goToLocation = location;
                    return location;
                }
            }
            return "";
        }

        public static void GoToLocation(string location)
        {
            currentLocation = AdvenureGame.parser.CreateLocationFromDescription(goToLocation);
        }


    }

}