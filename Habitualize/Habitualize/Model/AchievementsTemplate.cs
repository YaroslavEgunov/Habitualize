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


        public bool Unlocked = false;

        public bool IsAchievementComplete(AchievementsTemplate achievement, List<HabitTemplate> habits)
        {
            switch (Description)
            {
                case "Read 1 book":
                    return habits.OfType<Reading>().Any(b => b.BookComplete);
                case "Read 10 books":
                    return habits.OfType<Reading>().Count(b => b.BookComplete) >= 10;
                case "Read 100 books":
                    return habits.OfType<Reading>().Count(b => b.BookComplete) >= 100;
                case "Read 100 pages":
                    return habits.OfType<Reading>().Sum(b => b.PagesRead) >= 100;
                case "Read 1000 pages":
                    return habits.OfType<Reading>().Sum(b => b.PagesRead) >= 1000;
                case "Read 5202 pages":
                    return habits.OfType<Reading>().Sum(b => b.PagesRead) >= 5202;
                case "Read 8500 pages":
                    return habits.OfType<Reading>().Sum(b => b.PagesRead) >= 8500;
                case "Complete 1 training":
                    return habits.OfType<Training>().Any(t => t.CurrentWeight == t.TargetWeight);
                case "Complete 30 trainings":
                    return habits.OfType<Training>().Count(t => t.CurrentWeight == t.TargetWeight) >= 30;
                case "Complete 365 trainings":
                    return habits.OfType<Training>().Count(t => t.CurrentWeight == t.TargetWeight) >= 365;
                case "Take care of 1 plant":
                    return habits.OfType<Gardening>().Count() >= 1;
                case "Take care of 10 plants":
                    return habits.OfType<Gardening>().Count() >= 10;
                case "Water your plant":
                    return habits.OfType<Gardening>().Any(p => p.PlantIsWatered);
                default:
                    return false;
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
