using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    public class DailyTasks
    {
        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public Func<HabitTemplate, bool> Condition { get; set; }

        public bool IsTaskComplete(HabitTemplate habit)
        {
            return Condition(habit);
        }
    }
}
