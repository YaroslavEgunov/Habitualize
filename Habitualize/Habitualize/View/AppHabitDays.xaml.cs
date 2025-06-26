using Microsoft.Maui.Controls;
#if ANDROID
using static AndroidX.Core.Text.Util.LocalePreferences.FirstDayOfWeek;
#endif
using System.Collections.ObjectModel;
using static System.Collections.Specialized.NameObjectCollectionBase;
using System.Globalization;
using Microsoft.Maui.Storage;

namespace Habitualize.View;

public partial class AppHabitDays : ContentView
{
    //For test
    private void ResetProgress()
    {
        var keyDay = string.Format(HabitProgressKey, _habitTitle) + "_DayIndex";
        var keyDate = string.Format(HabitProgressKey, _habitTitle) + "_StartDate";
        Preferences.Remove(keyDay);
        Preferences.Remove(keyDate);
        _startDate = DateTime.MinValue;
        _currentDayIndex = 0;
    }

    //For test
    private void OnResetProgressClicked(object sender, EventArgs e)
    {
        ResetProgress();
        
        _currentDayIndex = 0;
        _startDate = DateTime.Now.Date;
        foreach (var day in Days)
        {
            day.IsActive = false;
            day.IsDone = false;
        }
        DaysCollection.ItemsSource = null;
        DaysCollection.ItemsSource = Days;
        StartButton.IsVisible = true;

        _midnightTimer?.Stop();
        _midnightTimer?.Dispose();
    }

    //For test
    public void CompleteAllDaysForTest(object sender, EventArgs e)
    {
        _startDate = DateTime.Now.Date.AddDays(-30); 
        _currentDayIndex = Days.Count - 1;
        foreach (var day in Days)
        {
            day.IsActive = true;
            day.IsDone = true;
        }
        SaveProgress();
        DaysCollection.ItemsSource = null;
        DaysCollection.ItemsSource = Days;
        UpdateResultButtonState();
    }

    private const string HabitProgressKey = "HabitProgress_{0}";
    private System.Timers.Timer _midnightTimer;
    private MainPage _mainPage;
    private DateTime _startDate;
    private string _habitTitle;
    private int _currentDayIndex = 0;
    private static readonly Dictionary<string, string[]> HabitTasks = new()
    {
        ["Drink water"] = Enumerable.Range(1, 30)
            .Select(i =>
                i == 10
                    ? "Celebrate yourself! 🎉 Today is your personal holiday, but don't forget about water!"
                    : i % 7 == 0
                        ? $"Drink {i * 50 + 1000} ml of water with lemon"
                        : $"Drink {i * 50 + 1000} ml of water")
            .ToArray(),
        ["Morning exercises"] = Enumerable.Range(1, 30)
            .Select(i =>
                i == 10
                    ? "Celebrate yourself! 🎉 Today is your personal holiday, you can rest!"
                    : i % 4 == 0
                        ? "Day off — rest and recover"
                        : $"Do {i + 5} minutes of exercise")
            .ToArray(),
        ["Brush teeth"] = Enumerable.Range(1, 30)
            .Select(i =>
                i == 10
                    ? "Celebrate yourself! 🎉 Today is your personal holiday, but don't forget brush teeth!"
                    : i % 5 == 0
                        ? $"Brush your teeth and use mouthwash (day {i})"
                        : $"Brush your teeth (day {i})")
            .ToArray()
    };

    public ObservableCollection<HabitDay> Days { get; set; }

    public class HabitDay
    {
        public int DayNumber { get; set; }
        public string Task { get; set; }
        public bool IsDone { get; set; }
        public bool IsActive { get; set; }
    }

    public AppHabitDays(string habitTitle, MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage;
        _habitTitle = habitTitle;
        HabitTitleLabel.Text = _habitTitle;

        LoadProgress();

        Days = new ObservableCollection<HabitDay>();
        var tasks = HabitTasks.ContainsKey(_habitTitle) ? HabitTasks[_habitTitle] : Enumerable.Range(1, 30).Select(i => $"Task for day: ").ToArray();
        
        for (int i = 1; i <= 30; i++)
        {
            Days.Add(new HabitDay
            {
                DayNumber = i,
                Task = tasks[i - 1],
                IsDone = false,
                IsActive = false
            });
        }

        if (!string.IsNullOrEmpty(_loadedDoneStates))
        {
            var doneArr = _loadedDoneStates.Split(',');
            for (int i = 0; i < Days.Count && i < doneArr.Length; i++)
                Days[i].IsDone = doneArr[i] == "1";
        }

        if (_startDate == DateTime.MinValue)
        {
            StartButton.IsVisible = true;
        }
        else
        {
            int daysPassed = (int)(DateTime.Now.Date - _startDate).TotalDays;
            for (int i = 0; i <= Math.Min(daysPassed, Days.Count - 1); i++)
                Days[i].IsActive = true;

            _currentDayIndex = Math.Min(daysPassed, Days.Count - 1);
            StartButton.IsVisible = false;
            StartMidnightTimer();
        }

        DaysCollection.ItemsSource = Days;
    }

    private void OnStartButtonClicked(object sender, EventArgs e)
    {
        StartButton.IsVisible = false;
        if (Days.Count > 0)
        {
            Days[0].IsActive = true;
            _currentDayIndex = 0;
            _startDate = DateTime.Now.Date;
            SaveProgress();
        }
        DaysCollection.ItemsSource = null;
        DaysCollection.ItemsSource = Days;
        StartMidnightTimer();
    }

    public void OpenNextDay(int currentDayIndex)
    {
        if (currentDayIndex + 1 < Days.Count)
        {
            Days[currentDayIndex].IsActive = false;

            Days[currentDayIndex + 1].IsActive = true;
            _currentDayIndex = currentDayIndex + 1;
            SaveProgress();
            DaysCollection.ItemsSource = null;
            DaysCollection.ItemsSource = Days;
        }
        UpdateResultButtonState();
    }

    private void StartMidnightTimer()
    {
        _midnightTimer?.Stop();
        _midnightTimer?.Dispose();

        var now = DateTime.Now;
        var nextMidnight = now.Date.AddDays(1);
        var msToMidnight = (nextMidnight - now).TotalMilliseconds;

        _midnightTimer = new System.Timers.Timer(msToMidnight);
        _midnightTimer.Elapsed += (s, e) =>
        {
            _midnightTimer.Stop();
            Device.BeginInvokeOnMainThread(() =>
            {
                OpenNextDay(_currentDayIndex);
                _currentDayIndex++;
            });
            _midnightTimer.Interval = 24 * 60 * 60 * 1000;
            _midnightTimer.Start();
        };
        _midnightTimer.AutoReset = false;
        _midnightTimer.Start();
    }

    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        _mainPage.ChangeContentWithAnimation(new AppThird(_mainPage));
    }

    private void UpdateResultButtonState()
    {
        if (_startDate != DateTime.MinValue && (DateTime.Now.Date - _startDate).TotalDays >= 30)
            ResultButton.IsEnabled = true;
        else
            ResultButton.IsEnabled = false;
    }

    private async void OnResultButtonClicked(object sender, EventArgs e)
    {
        int total = Days.Count;
        int done = Days.Count(d => d.IsDone);
        double percent = total == 0 ? 0 : (done * 100.0 / total);

        string message;
        if (percent <= 20)
            message = "You've just started! Keep going, you can do it!";
        else if (percent <= 50)
            message = "Not bad, but you can do better! Don't give up!";
        else if (percent <= 70)
            message = "Not bad, but you can do better! Don't give up!";
        else if (percent <= 90)
            message = "Not bad, but you can do better! Don't give up!";
        else 
            message = "Congratulations! You've completed the task!";

        await Application.Current.MainPage.DisplayAlert("Результаты",
            $"Completed {done} out of {total} days ({percent:0}%)\n\n{message}", "OK");
    }

    private void SaveProgress()
    {
        Preferences.Set(string.Format(HabitProgressKey, _habitTitle) + "_DayIndex", _currentDayIndex);
        Preferences.Set(string.Format(HabitProgressKey, _habitTitle) + "_StartDate", _startDate.Ticks);

        var doneStates = Days.Select(d => d.IsDone ? "1" : "0");
        Preferences.Set(string.Format(HabitProgressKey, _habitTitle) + "_DoneStates", string.Join(",", doneStates));
    }

    private void LoadProgress()
    {
        var keyDay = string.Format(HabitProgressKey, _habitTitle) + "_DayIndex";
        var keyDate = string.Format(HabitProgressKey, _habitTitle) + "_StartDate";
        var keyDone = string.Format(HabitProgressKey, _habitTitle) + "_DoneStates";
        _currentDayIndex = Preferences.Get(keyDay, 0);
        long startTicks = Preferences.Get(keyDate, DateTime.MinValue.Ticks);
        _startDate = new DateTime(startTicks);

        _loadedDoneStates = Preferences.Get(keyDone, "");
    }
    private string _loadedDoneStates = "";

    private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        SaveProgress();
    }
}

public class BoolToOpacityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (value is bool b && b) ? 1.0 : 0.3;
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
