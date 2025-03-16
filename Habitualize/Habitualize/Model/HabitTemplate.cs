// Ignore Spelling: Habitualize

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    public class HabitTemplate
    {
        private string _habitName;

        private string _habitDescription;

        public string HabitName 
        {
            get => _habitName;
            set
            {
                Validator.LessThanNCharacters(value, 100, nameof(HabitName));
                _habitName = value;
            }
        }

        public string HabitDescription 
        {
            get => _habitDescription;
            set
            {
                Validator.LessThanNCharacters(value, 300, nameof(HabitDescription));
                _habitDescription = value;
            }
        }

        public DateOnly HabitStartTime = DateOnly.FromDateTime(DateTime.Today);
      
        public string[] Tasks { get; set; }

        public DateOnly RepeatSchedule { get; set; }

        public int TotalDaysDone = 0;

        public TimeOnly TotalTimeSpentOnHabit;
    }
}
