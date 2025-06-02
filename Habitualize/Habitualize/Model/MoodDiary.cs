using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    public enum Mood
    {
        Awful,
        Bad,
        Neutral,
        Good,
        Great
    }

    public class MoodDiary
    {
        public string CurrentMood { get; set; } = Mood.Neutral.ToString();

        public DateTime Date { get; set; } = DateTime.Now;

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
