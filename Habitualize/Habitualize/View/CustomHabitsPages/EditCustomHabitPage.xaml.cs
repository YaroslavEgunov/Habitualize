using Habitualize.Model;
using Habitualize.View;
using Newtonsoft.Json;

namespace Habitualize.View.CustomHabitsPages;

public partial class EditCustomHabitPage : ContentPage
{
    private HabitTemplate _editedHabit = new HabitTemplate();

    private HabitTemplate _existingHabit;

    private void DeleteAtIndex(List<HabitTemplate> allHabits)
    {
        int indexToRemove = allHabits.FindIndex
                (h => h.HabitName == _existingHabit.HabitName &&
                h.HabitDescription == _existingHabit.HabitDescription &&
                h.IsComplete == _existingHabit.IsComplete);
        if (indexToRemove != -1)
        {
            allHabits.RemoveAt(indexToRemove);
        }
        if (_existingHabit.IsComplete && DatePick.Date == DateTime.Now.Date)
        {
            _existingHabit.RepeatSchedule = DateTime.Now.AddDays(2);
            DatePick.Date = DateTime.Now.AddDays(2);
            _existingHabit.IsComplete = false;
        }
    }

    public EditCustomHabitPage(HabitTemplate habit)
	{
		InitializeComponent(); 
        _existingHabit = JsonConvert.DeserializeObject<HabitTemplate>(JsonConvert.SerializeObject(habit));
        _editedHabit = habit;
        BindingContext = _editedHabit;
        NavigationPage.SetHasNavigationBar(this, false);
    }
    private async void OnConfirmButtonClicked(object sender, EventArgs e)
    {
        if (_editedHabit == _existingHabit)
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            await Navigation.PushAsync(new CustomHabitPage(existingHabits.OfType<HabitTemplate>().ToList()));
        }
        else
        {
            MainPage.Dailies[2].Condition = () => _editedHabit.IsComplete == true;
            if (MainPage.Dailies[2].IsTaskComplete())
            {
                ProgressionSystem.Experience += 100;
                if(ProgressionSystem.IsLevelUp())
                {
                    this.DisplayAlert("Congratulations!", $"Your level is now: {ProgressionSystem.Level}!", "OK");
                    AppMap.SavingLoadingSystem.SaveProgress();
                }
            }
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            var existingCustomHabits = existingHabits.OfType<HabitTemplate>().ToList();
            if (_editedHabit.IsComplete && DatePick.Date == DateTime.Now.Date)
            {
                _existingHabit.RepeatSchedule = DateTime.Now.AddDays(2);
                DatePick.Date = DateTime.Now.AddDays(2);
                _existingHabit.IsComplete = false;
            }
            DeleteAtIndex(existingCustomHabits);
            existingCustomHabits.Add(_editedHabit);
            await MainPage.CheckAchievements(existingCustomHabits, this);
            await MainPage.SavingLoadingSystem.SaveHabits(existingCustomHabits);
            await Navigation.PushAsync(new CustomHabitPage(existingCustomHabits));
        }
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        await Navigation.PushAsync(new CustomHabitPage(existingHabits.OfType<HabitTemplate>().ToList()));
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Delete this cutie", "You sure you want to delete this plant?", "Yep", "Nopers");
        if (result)
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            var existingCustomHabits = existingHabits.OfType<HabitTemplate>().ToList();
            DeleteAtIndex(existingCustomHabits);
            for (int i = 0; i < existingHabits.Count; i++)
            {
                if (existingHabits[i] is HabitTemplate)
                {
                    existingHabits.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < existingCustomHabits.Count; i++)
            {
                existingHabits.Add(existingCustomHabits[i]);
            }
            await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
            await Navigation.PushAsync(new CustomHabitPage(existingCustomHabits));
        }
        else
        {
            await DisplayAlert("Canceled deletion", "Oh phew", "ok...");
            return;
        }
    }
}