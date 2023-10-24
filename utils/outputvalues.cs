

namespace OUTPUTVALUES
{

    public class OutputValues
    {


        public static Random random = new Random();

        public static String[] doesNothing = new String[] { "That does nothing", "That is not possible", "You can't do that", "That did not work" };
        public static String[] youDied = new String[] { "Ded", "You died 💀", "Hmm seems like u ded 💀", "Think ur ded buddy" };

        public static string ResponseDoesNothing()
        {
            return doesNothing[random.Next(doesNothing.Length)];
        }
        public static string ResponseDed()
        {
            return youDied[random.Next(youDied.Length)];
        }

        public static AppplicationStrings qualityOfLife = new AppplicationStrings
        {

            //Adventure
            //Assertion Keys
            AssertionKeyDesc = "Description",
            AssertionKeyStatus = "Status",
            AssertionKeyPlayer = "Player",
            AssertionKeyMove = "Move",
            AssertionKeyHPDec = "hp.dec",

            //Cheats
            tpAbyss = "motherlode",
            godmode = "godmode",
            
            //Pretty stuff
            AfterDesc = '-',
            WritingSymbol = '>'

        };

    }



    public class AppplicationStrings
    {
        //Adventure
        public string? AssertionKeyDesc { get ; set ; }
        public string? AssertionKeyStatus { get ; set ; }
        public string? AssertionKeyPlayer { get ; set ; }
        public string? AssertionKeyMove { get ; set ; }
        public string? AssertionKeyHPDec { get ; set ; }

        public string? tpAbyss { get ; set ; }
        public string? godmode { get ; set ; }


        public char AfterDesc { get; set; }
        public char WritingSymbol { get; set; }
    }



}