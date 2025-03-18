using Habitualize.Model;

namespace Habitualize.View;

public partial class AddPlantsPage : ContentPage
{
    public Gardening CurrentPlant = new Gardening();

    public AddPlantsPage()
    {
        InitializeComponent();
        BindingContext = CurrentPlant;
        DatePick.MinimumDate = DateTime.Now;
    }

    private async void OnAddTaskButtonClicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("New Task", "Enter task:");

        if (!string.IsNullOrEmpty(result))
        {
            CurrentPlant.Tasks.Add(result.Trim());
        }
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(HabitNameEntry.Text) || string.IsNullOrWhiteSpace(HabitDescriptionEntry.Text))
        {
            await DisplayAlert("Nuh uh!", "Please enter plant name and description", "Sowwy...");
            return; 
        }
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        existingHabits.Add(CurrentPlant);
        await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
        var allPlants = existingHabits.OfType<Gardening>().ToList();
        await Navigation.PushAsync(new PlantsPage(allPlants));
    }
    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        await Navigation.PushAsync(new PlantsPage(existingHabits.OfType<Gardening>().ToList()));
    }

}