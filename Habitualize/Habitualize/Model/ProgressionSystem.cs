using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    public static class ProgressionSystem
    {
        public static int Experience { get; set; } = 0;

        public static int ExpForLevel { get; set; } = 100;

        public static int Level { get; set; } = 1;

        public static bool IsLevelUp()
        {
            if(Experience >= ExpForLevel)
            {
                Level++;
                Experience = 0;
                ExpForLevel += 50;
                return true;
            }
            return false;
        }
    }

    public class ProgressionData
    {
        public int Experience { get; set; } = 0;

        public int ExpForLevel { get; set; } = 100;

        public int Level { get; set; } = 1;
    }
}
