using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
using Habitualize.SignPages;
using Plugin.Maui.Calendar.Models;
using System.Collections.ObjectModel;
using Habitualize.Model;
using System.ComponentModel;
//using static AndroidX.ViewPager.Widget.ViewPager;

namespace Habitualize.View;

public partial class AppSchedule : ContentView, INotifyPropertyChanged
{
    public EventCollection Events { get; set; } = new EventCollection();

    private List<HabitTemplate> _habits;

    private DateTime _selectedDate = DateTime.Now.Date;

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                OnPropertyChanged();
                OnSelectedDateChanged();
            }
        }
    }

    public AppSchedule()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = this;
        UpdateHabitList(DateTime.Now);
    }

    private void OnSelectedDateChanged()
    {
        // Обновляем список привычек в зависимости от выбранной даты
        UpdateHabitList(SelectedDate);
    }

    private async void UpdateHabitList(DateTime date)
    {
        _habits = await MainPage.SavingLoadingSystem.LoadHabits();
        // Фильтруем привычки по выбранной дате
        var habitsForSelectedDate = _habits.Where(h => h.RepeatSchedule.Date == date.Date).ToList();
        Events.Clear();
        var sortedHabits = _habits.OrderBy(habit => habit.RepeatSchedule);
        var groupedHabits = sortedHabits.GroupBy(habit => habit.RepeatSchedule.Date).ToDictionary(group => group.Key, group => group.Select(habit => habit.HabitName).ToList());
        foreach(var entry in groupedHabits)
        {
            var dateInDict = entry.Key;
            var habitNames = entry.Value;
            Events.Add(dateInDict, habitNames);
        }

        HabitCollectionView.ItemsSource = habitsForSelectedDate;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}