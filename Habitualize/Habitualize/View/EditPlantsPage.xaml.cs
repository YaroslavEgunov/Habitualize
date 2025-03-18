using Habitualize.Model;
using Newtonsoft.Json;

namespace Habitualize.View;

public partial class EditPlantsPage : ContentPage
{
    private Gardening _editedPlant = new Gardening();

	private Gardening _existingPlant;

	public EditPlantsPage(Gardening plant)
	{
		InitializeComponent();
        _existingPlant = JsonConvert.DeserializeObject<Gardening>(JsonConvert.SerializeObject(plant));
        _editedPlant = plant;
        BindingContext = _editedPlant;
    }

    private async void OnConfirmButtonClicked(object sender, EventArgs e)
    {
        if(_editedPlant == _existingPlant)
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            await Navigation.PushAsync(new PlantsPage(existingHabits.OfType<Gardening>().ToList()));
        }
        else
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            var existingPlants = existingHabits.OfType<Gardening>().ToList();
            int indexToRemove = existingPlants.FindIndex
                (p => p.HabitName == _existingPlant.HabitName && 
                p.HabitDescription == _existingPlant.HabitDescription && 
                p.PlantIsWatered == _existingPlant.PlantIsWatered);
            if (indexToRemove != -1)
            {
                // Удаляем элемент по индексу
                existingPlants.RemoveAt(indexToRemove);
            }
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
            await Navigation.PushAsync(new PlantsPage(existingPlants));
        }
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        await Navigation.PushAsync(new PlantsPage(existingHabits.OfType<Gardening>().ToList()));
    }
}