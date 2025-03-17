// Ignore Spelling: Habitualize

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    public class HabitTemplate : INotifyPropertyChanged
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

        public DateOnly HabitStartTime { get; set; }
      
        public List<string> Tasks { get; set; }

        public DateOnly RepeatSchedule { get; set; }

        public int TotalDaysDone { get; set; }

        public TimeOnly TotalTimeSpentOnHabit { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
