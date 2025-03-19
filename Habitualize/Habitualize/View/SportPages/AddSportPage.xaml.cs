using Habitualize.Model;
using Habitualize.View;

namespace Habitualize.View;

public partial class AddSportPage : ContentPage
{
	public Training CurrentTraining = new Training();

	public AddSportPage()
	{
		InitializeComponent();
		BindingContext = CurrentTraining;
        DatePick.MinimumDate = DateTime.Now;
    }

    private async void OnAddExerciseButtonClicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("New Exercise", "Enter exercise:");

        if (!string.IsNullOrEmpty(result))
        {
            CurrentTraining.Tasks.Add(result.Trim());
        }
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TainingNameEntry.Text) || string.IsNullOrWhiteSpace(TrainingDescriptionEntry.Text) || string.IsNullOrWhiteSpace(TrainingWeightEntry.Text))
        {
            await DisplayAlert("Nuh uh!", "Please enter training, description and desired weight", "Sowwy...");
            return;
        }
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        existingHabits.Add(CurrentTraining);
        await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
        var allTraining = existingHabits.OfType<Training>().ToList();
        await Navigation.PushAsync(new SportPage(allTraining));
    }
    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        await Navigation.PushAsync(new SportPage(existingHabits.OfType<Training>().ToList()));
    }
}