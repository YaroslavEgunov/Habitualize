// Ignore Spelling: Habitualize

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public DateTime HabitStartTime { get; set; } = DateTime.Now;

        public ObservableCollection<string> Tasks { get; set; } = new ObservableCollection<string>();

        public DateTime RepeatSchedule { get; set; } = DateTime.Now;

        public int TotalDaysDone { get; set; }

        public DateTime TotalTimeSpentOnHabit { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
