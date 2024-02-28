using System.Collections.Generic;

namespace SinuousProductions
{
    public static class Utility
    {
        public static void Shuffle<T>(List<T> list)
        {
            System.Random random = new System.Random();
            int n = list.Count;
            for (int i = n - 1;  i > 0; i--)
            {
                int j = random.Next(i + 1);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }
    }
}
