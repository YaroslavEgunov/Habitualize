using Habitualize.Model;

namespace Habitualize.View;

public partial class AddPlantsPage : ContentPage
{
    public Gardening CurrentPlant = new Gardening
    {
        PlantImage = new Image()
    };

    public string NewTask = "";

    public AddPlantsPage()
    {
        InitializeComponent();
        BindingContext = CurrentPlant;
    }

    private async void OnSelectImageClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Choose plant image"
        });

        if (result != null)
        {
            using (var stream = await result.OpenReadAsync())
            {
                CurrentPlant.PlantImage.Source = ImageSource.FromStream(() => stream);
            }
        }
    }

    private async void OnAddTaskButtonClicked(object sender, EventArgs e)
    {

        string task = NewTask?.Trim();
        if (!string.IsNullOrEmpty(task))
        {
            CurrentPlant.Tasks.Add(task);
            NewTask = string.Empty; // Очистка поля ввода задачи
        }
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
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