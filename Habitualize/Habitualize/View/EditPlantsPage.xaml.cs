using Habitualize.Model;
using Newtonsoft.Json;

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
    }

	public EditPlantsPage(Gardening plant)
	{
		InitializeComponent();
        _existingPlant = JsonConvert.DeserializeObject<Gardening>(JsonConvert.SerializeObject(plant));
        _editedPlant = plant;
        BindingContext = _editedPlant;
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
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            var existingPlants = existingHabits.OfType<Gardening>().ToList();
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
            await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
            if (_editedPlant.PlantIsWatered && DatePick.Date == DateTime.Now.Date)
            {
                var newDate = DateTime.Now.AddDays(2);
                _existingPlant.RepeatSchedule = newDate;
                DatePick.Date = newDate;
            }
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
            DeleteAtIndex(existingPlants);
            for (int i = 0; i < existingHabits.Count; i++)
            {
                if (existingHabits[i] is Gardening)
                {
                    existingHabits.RemoveAt(i);
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