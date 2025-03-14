using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    class HabitTemplate
    {
        public string HabitName 
        {
            get
            {
                return HabitName;
            }
            set
            {
                Validator.LessThanNCharacters(value, 100, nameof(HabitName));
                HabitName = value;
            }
        }

        public string HabitDescription 
        {  
            get
            {
                return HabitDescription;
            }
            set
            {
                Validator.LessThanNCharacters(value, 300, nameof(HabitDescription));
                HabitDescription = value;
            }
        }

        public DateOnly HabitStartTime 
        {
            get
            {
                return HabitStartTime;
            }
            set
            {
                HabitStartTime = value;
            }
        } 

        public string[] Tasks { get; set; }

        public DateOnly RepeatSchedule { get; set; }

        public int TotalDaysDone = 0;

        public TimeOnly TotalTimeSpentOnHabit;
    }
}
