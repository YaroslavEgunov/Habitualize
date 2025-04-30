using Habitualize.Model;
namespace Habitualize.View.CustomHabitsPages;

public partial class AddCustomHabitPage : ContentPage
{
    public HabitTemplate CurrentHabit = new HabitTemplate();

    public AddCustomHabitPage()
    {
        InitializeComponent();
        BindingContext = CurrentHabit;
        DatePick.MinimumDate = DateTime.Now;
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void OnAddTaskButtonClicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("New Task", "Enter task:");

        if (!string.IsNullOrEmpty(result))
        {
            CurrentHabit.Tasks.Add(result.Trim());
        }
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(HabitNameEntry.Text) || string.IsNullOrEmpty(HabitDescriptionEntry.Text))
        {
            await DisplayAlert("Nuh uh!", "Please enter habit name and description", "Sowwy...");
            return;
        }
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        existingHabits.Add(CurrentHabit);
        await MainPage.CheckAchievements(existingHabits, this);
        await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
        var allHabits = existingHabits.OfType<HabitTemplate>().ToList();
        await Navigation.PushAsync(new CustomHabitPage(allHabits));
    }
    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        await Navigation.PushAsync(new CustomHabitPage(existingHabits.OfType<HabitTemplate>().ToList()));
    }
}