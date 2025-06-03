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

        public Func<bool> Condition { get; set; }

        public bool IsTaskComplete()
        {
            return Condition();
        }
    }
}
