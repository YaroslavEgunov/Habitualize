using Habitualize.Model;
using Newtonsoft.Json;
using System;

namespace Habitualize.View;

public partial class EditPlantsPage : ContentPage
{
    private Gardening _editedPlant = new Gardening();

	private Gardening _existingPlant;

    private void DeleteAtIndex(List<Gardening> allPlants)
    {
        int indexToRemove = allPlants.FindIndex
                (p => p.HabitName == _existingPlant.HabitName &&
                p.HabitDescription == _existingPlant.HabitDescription &&
                p.PlantIsWatered == _existingPlant.PlantIsWatered);
        if (indexToRemove != -1)
        {
            allPlants.RemoveAt(indexToRemove);
        }
        if (_existingPlant.PlantIsWatered && DatePick.Date == DateTime.Now.Date)
        {
            _existingPlant.RepeatSchedule = DateTime.Now.AddDays(2);
            DatePick.Date = DateTime.Now.AddDays(2);
            _existingPlant.PlantIsWatered = false;
        }
    }

	public EditPlantsPage(Gardening plant)
	{
		InitializeComponent();
        _existingPlant = JsonConvert.DeserializeObject<Gardening>(JsonConvert.SerializeObject(plant));
        _editedPlant = plant;
        BindingContext = _editedPlant;
        NavigationPage.SetHasNavigationBar(this, false);

    }

    private async void OnConfirmButtonClicked(object sender, EventArgs e)
    {
        if (_editedPlant == _existingPlant)
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            await Navigation.PushAsync(new PlantsPage(existingHabits.OfType<Gardening>().ToList()));
        }
        else
        {
            MainPage.Dailies[0].Condition = () => _editedPlant.PlantIsWatered == true;
            if (MainPage.Dailies[0].IsTaskComplete())
            {
                ProgressionSystem.Experience += 100;
            }
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            var existingPlants = existingHabits.OfType<Gardening>().ToList();
            if (_editedPlant.PlantIsWatered && DatePick.Date == DateTime.Now.Date)
            {
                _existingPlant.RepeatSchedule = DateTime.Now.AddDays(2);
                DatePick.Date = DateTime.Now.AddDays(2);
                _existingPlant.PlantIsWatered = false;
            }
            DeleteAtIndex(existingPlants);
            existingPlants.Add(_editedPlant);
            int j = 0;
            for (int i = 0; i < existingHabits.Count; i++)
            {
                if (existingHabits[i] is Gardening)
                {
                    existingHabits[i] = existingPlants[j];
                    j++;
                }
            }
            await MainPage.CheckAchievements(existingHabits, this);
            await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
            await Navigation.PushAsync(new PlantsPage(existingPlants));
        }
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        await Navigation.PushAsync(new PlantsPage(existingHabits.OfType<Gardening>().ToList()));
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Delete this cutie", "You sure you want to delete this plant?", "Yep", "Nopers");
        if(result)
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            var existingPlants = existingHabits.OfType<Gardening>().ToList();
            //plants without deleted one
            DeleteAtIndex(existingPlants);
            for (int i = 0; i < existingHabits.Count; i++)
            {
                if (existingHabits[i] is Gardening)
                {
                    existingHabits.RemoveAt(i);
                    i--;
                }
            }
            for(int i = 0; i < existingPlants.Count;i++)
            {
                existingHabits.Add(existingPlants[i]);
            }
            await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
            await Navigation.PushAsync(new PlantsPage(existingPlants));
        }
        else
        {
            await DisplayAlert("Canceled deletion", "Oh phew", "ok...");
            return;
        }
    }
}