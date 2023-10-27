
using Adventure;
using Adventure.BuildingBlocks;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Transactions;
using OUTPUTVALUES;
using System.Security.Cryptography.X509Certificates;
using DEBUFF;
using System.Reflection;

namespace COMMANDS
{

    public class Commands
    {
        static Location currentLocation;
        public const string ABYSS = "game/abyss.adv";
        public const string START = "game/start.adv";
        public static List<List<string>> ValidCommands = new List<List<string>>() { DebuffCommands, TPCommands };
        public static List<string> DebuffCommands = new List<string>() { "cleanse", "poision", "cold" };
        public static List<string> TPCommands = new List<string>() { "abyss", "start" };

        public static Dictionary<string, string> LookUpLocation = new Dictionary<string, string>();
        public static string goToLocation = string.Empty;


        // godModeCheat

        public Commands()
        {
            var type = Type.GetType("godModeCheat");
            var myMethod = type.GetMethod("MyMethod");
            //LookUpLocation.Add("abyss", ABYSS);

            //GET DICTIONARY TO WORK
        }

        public static string CheckCommand(string command, string typeOfCOmmand = "")
        {
            ///Type of command
            ///If its a debuff command, godmode, TP etc.
            string findCommand = command.ToLower();
            string type = typeOfCOmmand.ToLower();
            for (int i = 0; i < ValidCommands.Count; i++)
            {
                if (findCommand.Contains("debuff"))
                {
                    for (int j = 0; j < DebuffCommands.Count; j++)
                    {
                        if (findCommand.Contains(DebuffCommands[j]))
                        {

                            return "FOUND IT!!!   " + command;

                        }
                    }
                }
                else if (findCommand.Contains("tp"))
                {
                    for (int j = 0; j < TPCommands.Count; j++)
                    {
                        if (findCommand.Contains(TPCommands[j]))
                        {
                            return "FOUND IT!!!   " + command;
                        }
                    }
                }
            }

            return "not found :C";
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
            foreach (string element in TPCommands)
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