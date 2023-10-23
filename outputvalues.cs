

namespace OUTPUTVALUES {

    public class OutputValues {
        
        public static Random random = new Random();
        
        public static String[] doesNothing = new String[]{"That does nothing", "That is not possible", "You can't do that", "That did not work"};
        public static String[] youDied = new String[]{"Ded", "You died 💀", "Hmm seems like u ded 💀", "Think ur ded buddy"};

        public static string ResponseDoesNothing() {
            return doesNothing[random.Next(doesNothing.Length)];
        }
        public static string ResponseDed() {
            return youDied[random.Next(youDied.Length)];
        }

    }



}