
using System.Text.RegularExpressions;
using static System.Console;
using static Utils.ANSICodes;
using static Utils.ANSICodes.BgColors;
using static Utils.ANSICodes.Colors;
using static Utils.ANSICodes.Effects;
using static Utils.ANSICodes.Positioning;
using static Adventure.AssetsAndSettings;


namespace Utils
{
    public enum Alignment : int
    {
        LEFT,
        RIGHT,
        CENTER
    }

    public static class Output
    {
        public static string Color(string text, string color)
        {
            return $"{color}{text}";
        }
        public static string Bold(string text)
        {
            return $"{Effects.Bold}{text}";
        }
        public static string Reset(string text)
        {
            return $"{text}{ANSICodes.Reset}";
        }

        public static string AddColor(string text, string color, bool reset = true, bool oneLine = true)
        {
            return $"{color}{text}{ANSICodes.Reset}";

        }
        public static string Write(string text, bool newLine = false)
        {
            /// TODO: There is a line limit, but it is not enforced in this function. It should be. 
            
            string[] outputString = text.Split();
            int spaceCount = 0;

            if (newLine)
            {
                Console.WriteLine(text);
            }
            else
            {
                //Console.Write(text);
                foreach (string element in outputString) {
                    Console.Write(element + " ");
                    if(spaceCount > 50) {
                        Console.WriteLine("\n");
                        spaceCount = 0;
                    } else {
                        spaceCount++;
                    }
                }
            }
            return text;
        }
        public static string Align(string text, Alignment alignment = Alignment.LEFT, bool newLine = false)
        {
            if (text.Split("\n").Length > 1)
            {
                string[] lines = text.Split("\n");
                List<string> lineSegments = new List<string>();
                foreach (string line in lines)
                {
                    lineSegments.Add(Align($"{line}", alignment));
                }
                return string.Join("", lineSegments);
            }

            if (alignment != Alignment.LEFT)
            {
                int width = System.Console.WindowWidth;
                int textWidth = text.Length;
                int paddingLeft = 0;
                int paddingRight = 0;

                if (alignment == Alignment.CENTER)
                {
                    paddingRight = paddingLeft = (width - textWidth) / 2;
                }
                else if (alignment == Alignment.RIGHT)
                {
                    paddingLeft = width - textWidth;
                }

                paddingLeft = Math.Clamp(paddingLeft, 0, int.MaxValue);
                paddingRight = Math.Clamp(paddingRight, 0, int.MaxValue);
                text = $"{new string(' ', paddingLeft)}{text}{new string(' ', paddingRight)}";
            }

            if (newLine) { text += "\n"; }

            return text;
        }
        public static string ColorizeWords(string text, string primaryColor, string secondaryColor)
        {
            string modifiedInput = secondaryColor + text + ANSICodes.Reset;

            return Regex.Replace(
                modifiedInput,
                @"\*(.*?)\*",
                match => ANSICodes.Reset + primaryColor + match.Groups[1].Value + ANSICodes.Reset + secondaryColor
            );
        }

    }

}
