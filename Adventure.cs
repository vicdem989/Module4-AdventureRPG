using System.Collections;
using Utils;
using static Utils.Constants;
using Adventure.BuildingBlocks;
using static Utils.Output;
using static Adventure.AssetsAndSettings;
using OUTPUTVALUES;
using System.Reflection;
using DEBUFF;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using COMMANDS;
using INVENTORY;
using System.Threading.Channels;
using System.Collections.Concurrent;
using System.Data.Common;
using ITEMCUSTOMDESCRIPTION;
using System.ComponentModel;


namespace Adventure
{
    public class AdvenureGame : IGameScreen
    {
        const int PADDING = 3;
        int startRow = 5;
        int startColumn = (int)((Console.WindowWidth - MAX_LINE_WIDTH) * 0.5);


        public static Player hero;
        public static ItemDusctomDescription CustomDesc;

        #region  Basic Commands -------------------------------------------------------------------------------

        const string QUIT_COMMAND = "quit";
        const string CLEAR_COMMAND = "clear";
        const string HELP_COMMAND = "help";
        const string LOOK_COMMAND = "look";
        const string BAG_COMMAND = "inv";
        Dictionary<string, Action<AdvenureGame>> basicCommands = new Dictionary<string, Action<AdvenureGame>>()
        {
            [QUIT_COMMAND] = (game) => { game.OnExitScreen(typeof(SplashScreen), new object[] { AssetsAndSettings.SPLASH_ART_FILE, true }); },
            [CLEAR_COMMAND] = (game) => { Console.Clear(); },
            [HELP_COMMAND] = (game) => { game.currentDescription = "///TODO: This should print a helpfull message, maybe a list of commands? But it is not."; },
            [LOOK_COMMAND] = (game) => { game.currentDescription = game.currentLocation.Description; },
            [BAG_COMMAND] = (game) => { game.currentDescription = hero.InventoryDisplay(); }
        };




        #endregion

        string commandBuffer = string.Empty;
        static string command = string.Empty;
        string currentDescription = "";
        Location currentLocation;
        bool dirty = true;
        public Action<Type, Object[]> OnExitScreen { get; set; }
        public static Adventure.Parser parser = new();
        string locationDescription = string.Empty;

        const string GODMODE_CHEAT = "/godmode";

        Dictionary<string, Action<AdvenureGame>> cheatCommands = new Dictionary<string, Action<AdvenureGame>>()
        {
            [GODMODE_CHEAT] = (game) => { game.currentDescription = Commands.godModeCheat(command, hero); }
        };

        public void Init()
        {
            command = commandBuffer = String.Empty;
            currentLocation = parser.CreateLocationFromDescription(AssetsAndSettings.GAME_SOURCE);
            currentDescription = currentLocation.Description;
            hero = new Player();
            CustomDesc = new ItemDusctomDescription();
        }
        public void Input()
        {
            if (!Console.KeyAvailable)
                return;

            dirty = true;
            ConsoleKeyInfo keyInfo = Console.ReadKey(false);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                command = commandBuffer.ToLower();
                commandBuffer = String.Empty;
            }
            else if (keyInfo.Key == ConsoleKey.Backspace && commandBuffer.Length >= 1)
            {
                commandBuffer = commandBuffer.Substring(0, commandBuffer.Length - 1);
            }
            else
            {
                commandBuffer += keyInfo.KeyChar;
            }

        }

        public void Update()
        {

            /*Write(Commands.CheckCommand("tp abyss"));
            return;*/
            ///TODO: refactor this function. i.e. make it more readable. 
            if (command == String.Empty)
                return;

            if (Debuff.playerDebuff)
            {
                hero.hp -= Debuff.DebuffTick();
            }
            if (cheatCommands.ContainsKey(command))
            {
                cheatCommands[command](this);
            }
            else if (command.Contains("test"))
            {
                //currentDescription = currentLocation.Inventory[; //ItemDusctomDescription.StartKey[0] + ItemDusctomDescription.StartWindow[0];
                /*string finalDescription = string.Empty;
                currentDescription = finalDescription;*/
                //currentDescription = currentLocation.LocationDescriptionType[0];// + "  " +  currentLocation.LocationDescriptionType[0];//ItemDusctomDescription.CreateCustomLocationDescription("A broken window") + ItemDusctomDescription.CreateCustomLocationDescription("Evil");
                if (currentLocation.LocationDescriptionType[0] != "standard")
                {
                    currentDescription = ItemDusctomDescription.CreateCustomLocationDescription("Evil Key");
                    Environment.Exit(0);
                }
                else
                {
                    currentDescription = currentLocation.Description;
                }
                return;
            }
            else if (command.Contains("/"))
            {
                string location = command.ToLower().Replace("/", " ").Trim(' ');

                if (Commands.VerifyLocation(location) != "")
                {
                    currentLocation = Commands.GoToLocation();
                    currentDescription = Commands.GoToLocation().Description;
                    return;
                }
            }
            if (basicCommands.ContainsKey(command))
            {
                basicCommands[command](this);
            }
            else if (command.Contains("delete"))
            {
                if (!hero.inventoryOpen)
                {
                    currentDescription = "Ye inventory needs to be open first... type *inv* to open";
                }
                else
                {
                    string[] commandParts = command.Split(" ", StringSplitOptions.TrimEntries);
                    string itemToRemove = commandParts[1];

                    if (commandParts.Length > 2)
                        return;
                    if (!hero.VerifyItemInInventory(itemToRemove))
                    {
                        currentDescription += "No such item in your inventory";
                    }
                    currentDescription = hero.InventoryRemove(itemToRemove);
                }
            }
            else
            {
                SetActionTargetDesc();
            }

            command = String.Empty;

            // This is not a good solution, rewrite for clearity.
            if (hero.hp <= 0)
            {
                currentDescription = OutputValues.ResponseDed(); //TODO: Remove magick string, les statick feadback?
                Environment.Exit(0);
            }
        }

        private void SetActionTargetDesc()
        {
            string actionDesc = "";
            string targetDesc = "";

            string[] commandParts = command.Split(" ", StringSplitOptions.TrimEntries);

            foreach (string item in commandParts)
            {
                if (currentLocation.keywords.Contains(item) && actionDesc == "")
                {
                    actionDesc = item;
                }
                else if (currentLocation.Inventory.Keys.Contains<string>(item) && targetDesc == "")
                {
                    targetDesc = item;
                }

                if (actionDesc != "" && targetDesc != "")
                {
                    break; // No longer anny point in staying in this for loop. 
                }
            }

            if (targetDesc == "" && actionDesc == "")
            {
                currentDescription = OutputValues.ResponseDoesNothing();//TODO: Remove magick string, make feadback less static?
            }
            else
            {
                SetDescAndTargetStatus(targetDesc, actionDesc);
            }
        }

        private void SetDescAndTargetStatus(string targetDesc, string actionDesc)
        {

            Item target = currentLocation.Inventory[targetDesc];
            string key = $"{target.Status}.{actionDesc}";


            if (!target.actions.Keys.Contains<string>(key))
            {
                currentDescription = OutputValues.ResponseDoesNothing();///TODO: Remove magick string, make feadback less static?
                return;
            }

            foreach (string assertion in target.actions[key])
            {
                string[] parts = assertion.Split(" => ", StringSplitOptions.TrimEntries);

                if (parts.Length < 2)
                    break;

                string assertionKey = parts[0];
                string assertionValue = parts[1];

                ///TODO: Remove magick key
                if (assertionKey == OutputValues.qualityOfLife.AssertionKeyDesc)
                {
                    currentDescription = assertionValue;
                }
                else if (assertionKey == OutputValues.qualityOfLife.AssertionKeyStatus)///TODO: Remove magick key
                {
                    target.Status = assertionValue;
                }
                else if (assertionKey == OutputValues.qualityOfLife.AssertionKeyPlayer && assertionValue == OutputValues.qualityOfLife.AssertionKeyHPDec) ///TODO: Remove magick string
                {
                    hero.hp -= int.Parse(currentLocation.Damage);
                }
                else if (assertionKey == "Debuff") ///TODO: Remove magick string
                {
                    string[] debuffParts = assertionValue.Split(" - ", StringSplitOptions.TrimEntries);
                    if (debuffParts.Length < 3)
                    {
                        break;
                    }

                    string debuffType = debuffParts[0];
                    int debuffDuration = int.Parse(debuffParts[1]);
                    int debuffDamage = int.Parse(debuffParts[2]);
                    if (Debuff.CheckDebuff(debuffType) == true)
                    {
                        Debuff.ApplyDebuff(debuffType, debuffDuration, debuffDamage);
                    }
                }
                else if (assertionKey == "Player")
                {
                    if (assertionValue == "Inventory.Add")
                    {
                        hero.InventoryAdd(target.Id, target.Damage, target.Description, target.Status, target.Type);
                    }
                    else if (assertionValue == "Inventory.Remove")
                    {
                        hero.InventoryRemove(target.Id);
                    }
                }
                else if (assertionKey == OutputValues.qualityOfLife.AssertionKeyMove) ///TODO: You know what to do. 
                {
                    Console.Clear();
                    Adventure.Parser parser = new();
                    currentLocation = parser.CreateLocationFromDescription($"game/{assertionValue}");
                    currentDescription = $"{currentDescription}\n{currentLocation.Description}";
                }
            }
        }


        public void Draw()
        {
            if (dirty)
            {
                dirty = false;
                int currentRow = startRow;
                int currentColumn = startColumn;
                Console.Clear();


                ///TODO: There is a problem when the description extends over  WORKING ON
                //PaddingCenter(currentRow, currentColumn);
                OutputDesc(currentRow, currentColumn);
                /// TODO: Magic string, fix DONE

                PaddingCenter(currentRow, currentColumn);
                if (Debuff.currentPlayerDebuff != "No debuff... yet...")
                {
                    Write("Current Status: " + AddColor(Debuff.currentPlayerDebuff, Debuff.preferredColor, true, false));
                }
                else
                {
                    Write("Current Status: " + AddColor(Debuff.currentPlayerDebuff, ANSICodes.Colors.White, true, false));
                }

                PaddingCenter(currentRow, currentColumn);
                /// TODO: Magic string, fix
                Write(OutputValues.qualityOfLife.WritingSymbol + $" {commandBuffer}" + "_");

                PaddingCenter(currentRow, currentColumn);
                Write("HP " + hero.hp + ": ");

                for (int i = 0; i < hero.hp; i++)
                {
                    if (hero.hp < 20)
                    {
                        Write(" ♥ ");
                    }
                    else
                    {
                        Write("Too much hp... ");
                        for (int j = 0; j < 3; j++)
                        {
                            Write(" ♥ ");
                        }
                        return;
                    }
                }
            }
        }

        private void OutputDesc(int currentRow, int currentColumn)
        {
            PaddingCenter(currentRow, currentColumn);
            Write(Reset(ColorizeWords(currentDescription + " ", ANSICodes.Colors.Blue, ANSICodes.Colors.Yellow)), newLine: false);
            /*char[] currDesc = currentDescription.ToArray();
            int spaceCount = 0;
            for (int i = 0; i < currDesc.Length; i++)
            {
                spaceCount++;
                //Console.ForegroundColor = ConsoleColor.Yellow;
                if (currDesc[i] == ' ' || i == currDesc.Length - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (currDesc[i] == '*')
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else if (currDesc[i] == '!')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (spaceCount > 96 && currDesc[i] == ' ')
                {
                    Write("\n");
                    PaddingCenter(currentRow, currentColumn);
                    spaceCount = 0;
                }
                else 
                {
                    Write(currDesc[i].ToString());
                }
            }
            Console.ResetColor();
            */
            PaddingCenter(currentRow, currentColumn);
            Write($"{new string(OutputValues.qualityOfLife.AfterDesc, MAX_LINE_WIDTH)}", newLine: true);
        }

        private void PaddingCenter(int currentRow, int currentColumn)
        {
            currentRow = Console.CursorTop + PADDING;
            Write(ANSICodes.Positioning.SetCursorPos(currentRow, currentColumn));
        }
    }
}

