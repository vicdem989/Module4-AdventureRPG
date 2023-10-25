﻿using System.Collections;
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


namespace Adventure
{
    public class AdvenureGame : IGameScreen
    {
        const int PADDING = 3;
        int startRow = 5;
        int startColumn = (int)((Console.WindowWidth - MAX_LINE_WIDTH) * 0.5);

        #region  Basic Commands -------------------------------------------------------------------------------

        const string QUIT_COMMAND = "quit";
        const string CLEAR_COMMAND = "clear";
        const string HELP_COMMAND = "help";
        const string LOOK_COMMAND = "look";
        Dictionary<string, Action<AdvenureGame>> basicCommands = new Dictionary<string, Action<AdvenureGame>>()
        {
            [QUIT_COMMAND] = (game) => { game.OnExitScreen(typeof(SplashScreen), new object[] { AssetsAndSettings.SPLASH_ART_FILE, true }); },
            [CLEAR_COMMAND] = (game) => { Console.Clear(); },
            [HELP_COMMAND] = (game) => { game.currentDescription = "///TODO: This should print a helpfull message, maybe a list of commands? But it is not."; },
            [LOOK_COMMAND] = (game) => { game.currentDescription = game.currentLocation.Description; }
        };

        #endregion

        string commandBuffer;
        string command;
        string currentDescription = "";
        Location currentLocation;
        bool dirty = true;
        public Action<Type, Object[]> OnExitScreen { get; set; }
        public static Adventure.Parser parser = new();

        public static Player hero;
        public void Init()
        {
            command = commandBuffer = String.Empty;
            currentLocation = parser.CreateLocationFromDescription(AssetsAndSettings.GAME_SOURCE);
            currentDescription = currentLocation.Description;
            hero = new Player();
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

            ///TODO: refactor this function. i.e. make it more readable. 
            if (command == String.Empty)
                return;

            if (Debuff.playerDebuff)
            {
                hero.hp -= Debuff.DebuffTick();
            }

            if (command == OutputValues.qualityOfLife.godmode)
            {
                hero.hp += 999;
                currentDescription = "That did not work, or did it?";
                command = string.Empty;
                return;
            }
            if (basicCommands.ContainsKey(command))
            {
                basicCommands[command](this);
            }
            else if (command == "abyss")//command.Contains("/"))
            {
                //Environment.Exit(0);
                string location = command.ToLower();//Remove(0).ToLower();
                if (Commands.VerifyLocation(command) != "")
                {
                    //Environment.Exit(0);
                    currentLocation = Commands.GoToLocation();
                    currentDescription = Commands.GoToLocation().Description;

                    return;
                }
                /*
                Check if list of commands exists
                User can input /tp (location)
                if part[0] == "/tp"
                    location = part[1].tolower()
                    moveTo(location) in cheat code class
                    moveTo(string location) {

                    }

                */
            }
            else
            {
                SetActionTargetDesc();
            }

            command = String.Empty;

            // This is not a good solution, rewrite for clearity.
            if (hero.hp < 0)
            {
                currentDescription += OutputValues.ResponseDed(); //TODO: Remove magick string, les statick feadback?
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
                else if (assertionKey == "Debuff" && Debuff.CheckDebuff(assertionValue) == true) ///TODO: Remove magick string
                {
                    hero.hp -= Debuff.ApplyDebuff(assertionValue);
                }
                else if (assertionKey == OutputValues.qualityOfLife.AssertionKeyMove) ///TODO: You know what to do. 
                {
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
                PaddingCenter(currentRow, currentColumn);

                ///TODO: There is a problem when the description extends over  WORKING ON
                OutputDesc(currentRow, currentColumn);

                PaddingCenter(currentRow, currentColumn);
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
                /// TODO: Magic string, fix DONE
                Write("HP:" + AddColor(" " + hero.hp, ANSICodes.Colors.Red, true, false) + " " + OutputValues.qualityOfLife.WritingSymbol + $" {commandBuffer}");
            }
        }

        private void OutputDesc(int currentRow, int currentColumn)
        {
            int spacesCount = 0;
            int stringLengthMin = 85;
            int stringLengthMax = 96;

            int maxWordsPerLine = 0;
            int outputLength = currentDescription.Length;
            PaddingCenter(currentRow, currentColumn);
            if (outputLength < stringLengthMax)
            {
                PaddingCenter(currentRow, currentColumn);
                Write(Reset(ColorizeWords(currentDescription, ANSICodes.Colors.Blue, ANSICodes.Colors.Yellow)), newLine: true);
                return;
            }
            Write($"{new string(OutputValues.qualityOfLife.AfterDesc, MAX_LINE_WIDTH)}", newLine: true);

            PaddingCenter(currentRow, currentColumn);
            char[] outputChar = currentDescription.ToCharArray();
            string[] output = currentDescription.Split(" ");
            for (int i = 0; i < output.Length; i++)
            {
                spacesCount++;
                if (spacesCount > maxWordsPerLine && output[i] == " ")
                {
                    spacesCount = 0;
                    PaddingCenter(currentRow, currentColumn);
                }
                Write(Reset(ColorizeWords(output[i].ToString() + " ", ANSICodes.Colors.Blue, ANSICodes.Colors.Yellow)), newLine: false);
            }


        }

        private void PaddingCenter(int currentRow, int currentColumn)
        {
            currentRow = Console.CursorTop + PADDING;
            Write(ANSICodes.Positioning.SetCursorPos(currentRow, currentColumn));
        }
    }
}

