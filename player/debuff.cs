

using Adventure;

namespace DEBUFF
{

    public class Debuff
    {

        public static void ColdDebuff(bool cold, int duration, int dmg)
        {
            if (cold)
            {
                for (int i = 0; i < duration; i++)
                {
                    AdvenureGame.hero.hp--;
                }
            }

        }

    }
}