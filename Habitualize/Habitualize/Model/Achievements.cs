using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    public class Achievements
    {
        public static List<AchievementsTemplate> AchievementsList = new List<AchievementsTemplate>
        {
            new AchievementsTemplate
            {
                Name = "Beginner Reader",
                Description = "Read 1 book",
                CompletionCondition = habits => habits.OfType<Reading>().Any(b => b.BookComplete)
            },
            new AchievementsTemplate
            {
                Name = "Reading Lover",
                Description = "Read 10 books",
                CompletionCondition = habits => habits.OfType<Reading>().Count(b => b.BookComplete ) >= 10
            },
            new AchievementsTemplate
            {
                Name = "Book Worm",
                Description = "Read 100 books",
                CompletionCondition = habits => habits.OfType<Reading>().Count(b => b.BookComplete ) >= 100
            },
            new AchievementsTemplate
            {
                Name = "Dipping Toes In",
                Description = "Read 100 pages",
                CompletionCondition = habits => habits.OfType<Reading>().Sum(b => b.PagesRead ) >= 100
            },
            new AchievementsTemplate
            {
                Name = "Time Well Spent",
                Description = "Read 1000 pages",
                CompletionCondition = habits => habits.OfType<Reading>().Sum(b => b.PagesRead ) >= 1000
            },
            new AchievementsTemplate
            {
                Name = "War And Peace Enjoyer",
                Description = "Read 5202 pages",
                CompletionCondition = habits => habits.OfType<Reading>().Sum(b => b.PagesRead ) >= 5202
            },
            new AchievementsTemplate
            {
                Name = "Reading A Whole Tree",
                Description = "Read 8500 pages",
                CompletionCondition = habits => habits.OfType<Reading>().Sum(b => b.PagesRead ) >= 8500
            },
            new AchievementsTemplate
            {
                Name = "Staying Healthy",
                Description = "Complete 1 training",
                CompletionCondition = habits => habits.OfType<Training>().Any(t => t.CurrentWeight == t.TargetWeight)
            },
            new AchievementsTemplate
            {
                Name = "Health Chaser",
                Description = "Complete 30 trainings",
                CompletionCondition = habits => habits.OfType<Training>().Count(t => t.CurrentWeight == t.TargetWeight) >= 30
            },
            new AchievementsTemplate
            {
                Name = "Bodybuilder",
                Description = "Complete 365 trainings",
                CompletionCondition = habits => habits.OfType<Training>().Count(t => t.CurrentWeight == t.TargetWeight) >= 365
            },
            new AchievementsTemplate
            {
                Name = "Green Thumb",
                Description = "Take care of 1 plant",
                CompletionCondition = habits => habits.OfType<Gardening>().Count() >= 1
            },
            new AchievementsTemplate
            {
                Name = "Home Garden",
                Description = "Take care of 10 plants",
                CompletionCondition = habits => habits.OfType<Gardening>().Count() >= 10
            },
            new AchievementsTemplate
            {
                Name = "Not Thirsty Anymore",
                Description = "Water your plant",
                CompletionCondition = habits => habits.OfType<Gardening>().Any(p => p.PlantIsWatered)
            },
            //new AchievementsTemplate
            //{
            //    Name = "My Home, My Rules",
            //    Description = "Make a habit of your own"
            //},
            //new AchievementsTemplate
            //{
            //    Name = "I Am My Own Master",
            //    Description = "Do your own habit for 30 days in a row"
            //},
            //new AchievementsTemplate
            //{
            //    Name = "Getting Better",
            //    Description = "Be registered for 30 days"
            //},
            //new AchievementsTemplate
            //{
            //    Name = "Almost Perfect",
            //    Description = "Be registered for 1 year"
            //},
            //new AchievementsTemplate
            //{
            //    Name = "Perfection Incarnate",
            //    Description = "Be registered for 2 years"
            //}
        };

        public void CheckAchievements(List<HabitTemplate> habits)
        {
            foreach(var achievement in AchievementsList)
            {
                achievement.IsAchievementComplete(habits);
            }
        }
    }
}
