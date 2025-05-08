using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    public class Achievements
    {
        public List<AchievementsTemplate> AchievementsList { get; set; }

        public Achievements()
        {
            AchievementsList = new List<AchievementsTemplate>
            {
                new AchievementsTemplate
                {
                    Name = "Beginner Reader",
                    Description = "Read 1 book",
                    Image = "achievement_books_one.png"
                },
                new AchievementsTemplate
                {
                    Name = "Reading Lover",
                    Description = "Read 10 books",
                    Image = "achievement_books_two.png"
                },
                new AchievementsTemplate
                {
                    Name = "Book Worm",
                    Description = "Read 100 books",
                    Image = "achievement_books_three.png"
                },
                new AchievementsTemplate
                {
                    Name = "Dipping Toes In",
                    Description = "Read 100 pages",
                    Image = "achievement_locked.png"
                },
                new AchievementsTemplate
                {
                    Name = "Time Well Spent",
                    Description = "Read 1000 pages",
                    Image = "achievement_locked.png"
                },
                new AchievementsTemplate
                {
                    Name = "War And Peace Enjoyer",
                    Description = "Read 5202 pages",
                    Image = "achievement_locked.png"
                },
                new AchievementsTemplate
                {
                    Name = "Reading A Whole Tree",
                    Description = "Read 8500 pages",
                    Image = "achievement_locked.png"
                },
                new AchievementsTemplate
                {
                    Name = "Staying Healthy",
                    Description = "Complete 1 training",
                    Image = "achievement_locked.png"
                },
                new AchievementsTemplate
                {
                    Name = "Health Chaser",
                    Description = "Complete 30 trainings",
                    Image = "achievement_locked.png"
                },
                new AchievementsTemplate
                {
                    Name = "Bodybuilder",
                    Description = "Complete 365 trainings",
                    Image = "achievement_locked.png"
                },
                new AchievementsTemplate
                {
                    Name = "Green Thumb",
                    Description = "Take care of 1 plant",
                    Image = "achievement_locked.png"
                },
                new AchievementsTemplate
                {
                    Name = "Home Garden",
                    Description = "Take care of 10 plants",
                    Image = "achievement_locked.png"
                },
                new AchievementsTemplate
                {
                    Name = "Not Thirsty Anymore",
                    Description = "Water your plant",
                    Image = "achievement_locked.png"
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
        }
    }
}
