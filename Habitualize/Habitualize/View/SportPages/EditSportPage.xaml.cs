using Habitualize.Model;
using Habitualize.View;
using Newtonsoft.Json;
using System.Drawing;

namespace Habitualize.View;

public partial class EditSportPage : ContentPage
{
    private Training _editedTraining = new Training();

    private Training _existingTraining;

    private void DeleteAtIndex(List<Training> allTraining)
    {
        int indexToRemove = allTraining.FindIndex
                (p => p.HabitName == _existingTraining.HabitName &&
                p.HabitDescription == _existingTraining.HabitDescription);
        if (indexToRemove != -1)
        {
            allTraining.RemoveAt(indexToRemove);
        }
    }

    public EditSportPage(Training training)
	{
		InitializeComponent();
        _existingTraining = JsonConvert.DeserializeObject<Training>(JsonConvert.SerializeObject(training));
        _editedTraining = training;
        BindingContext = _editedTraining;
        NavigationPage.SetHasNavigationBar(this, false);
        if (!(_existingTraining.CurrentWeight != _existingTraining.TargetWeight))
        {
            _existingTraining.LastTimeDone = DateTime.Now.Date;
            _existingTraining.TotalDaysDone++;
            _existingTraining.DaysDoneInARow++;
        }
        if (_existingTraining.CurrentWeight != _existingTraining.TargetWeight && _existingTraining.LastTimeDone < DateTime.Now.Date)
        {
            _existingTraining.DaysDoneInARow = 0;
        }
    }

    private async void OnConfirmButtonClicked(object sender, EventArgs e)
    {
        if (_editedTraining == _existingTraining)
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            await Navigation.PushAsync(new SportPage(existingHabits.OfType<Training>().ToList()));
        }
        else
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            var existingTrainings = existingHabits.OfType<Training>().ToList();
            if (_editedTraining.CurrentWeight == _editedTraining.TargetWeight && !_editedTraining.TrainingComplete)
            {
                _editedTraining.TrainingComplete = true;
            }
            DeleteAtIndex(existingTrainings);
            _editedTraining.LastTimeDone = DateTime.Now.Date;
            existingTrainings.Add(_editedTraining);
            int j = 0;
            for (int i = 0; i < existingHabits.Count; i++)
            {
                if (existingHabits[i] is Training)
                {
                    existingHabits[i] = existingTrainings[j];
                    j++;
                }
            }
            await MainPage.CheckAchievements(existingHabits, this);
            await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
            await Navigation.PushAsync(new SportPage(existingTrainings));
        }
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        await Navigation.PushAsync(new SportPage(existingHabits.OfType<Training>().ToList()));
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Delete this training", "You sure you want to delete this training?", "Yep", "Nope");
        if (result)
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            var existingTrainings = existingHabits.OfType<Training>().ToList();
            DeleteAtIndex(existingTrainings);
            for (int i = 0; i < existingHabits.Count; i++)
            {
                if (existingHabits[i] is Training)
                {
                    existingHabits.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < existingTrainings.Count; i++)
            {
                existingHabits.Add(existingTrainings[i]);
            }
            await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
            await Navigation.PushAsync(new SportPage(existingTrainings));
        }
        else
        {
            await DisplayAlert("Canceled deletion", "Fine I won't delete it, I promise", "ok...");
            return;
        }
    }
}