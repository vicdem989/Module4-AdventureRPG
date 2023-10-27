

namespace ITEMCUSTOMDESCRIPTION
{

    public class ItemDusctomDescription
    {
        //Start
        public static List<string> StartKey = new List<string> { "There is a *key* on the floor." };
        public static List<string> StartWindow = new List<string> { "Looks like there jlkdsadaskldhashdjahdsjk a *window*." };
        public static List<string> StartStudents = new List<string> { "You also see something resembling a *door*" };
        public static List<string> StartDoor = new List<string> { "Tehre is also a *trapdoor*." };
        public static List<string> StartTrapdoor = new List<string> { "Looks like some *students* are playing." };

        public static Dictionary<string, string> GetDescriptionByID { get; set; }

        public ItemDusctomDescription() {
            GetDescriptionByID = new Dictionary<string, string>();
            GetDescriptionByID.Add("Evil Key",  "There is a *key* on the floor." );
            GetDescriptionByID.Add("A broken window",  "Looks like there jlkdsadaskldhashdjahdsjk a *window*.");
        }

        public static string CreateCustomLocationDescription(string LocationID)
        {
            string newDescription = string.Empty;
            if (GetDescriptionByID.TryGetValue(LocationID, out string description))
            {
                newDescription += description;
            } 
            return newDescription;
        }

    }

}