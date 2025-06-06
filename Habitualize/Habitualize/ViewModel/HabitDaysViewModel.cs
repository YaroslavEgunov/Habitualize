using System.Collections.ObjectModel;

namespace Habitualize.ViewModel;

public class HabitDay
{
    public int DayNumber { get; set; }
    public string Task { get; set; }
    public bool IsDone { get; set; }
}

public class HabitDaysViewModel
{
    public string HabitTitle { get; set; }
    public ObservableCollection<HabitDay> Days { get; set; }

    public HabitDaysViewModel(string title)
    {
        HabitTitle = title;
        Days = new ObservableCollection<HabitDay>();
        for (int i = 1; i <= 30; i++)
        {
            Days.Add(new HabitDay
            {
                DayNumber = i,
                Task = $"Tasks for day {i}",
                IsDone = false
            });
        }
    }
}