using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    public class AchievementsTemplate : INotifyPropertyChanged
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public Func<List<HabitTemplate>, bool> CompletionCondition { get; set; }

        public bool Unlocked = false;

        public async void IsAchievementComplete(List<HabitTemplate> habits)
        {
            if(!Unlocked && CompletionCondition(habits))
            {
                Unlocked = true;
                await Application.Current.MainPage.DisplayAlert("Congratulations!", $"You completed achievement {Name} by doing this: {Description}", "Awesome");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
