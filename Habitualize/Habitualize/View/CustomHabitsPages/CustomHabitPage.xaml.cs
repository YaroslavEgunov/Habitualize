using Habitualize.Model;
namespace Habitualize.View.CustomHabitsPages;

public partial class CustomHabitPage : ContentPage
{
    public CustomHabitPage(List<HabitTemplate> data)
    {
        InitializeComponent();
        List<HabitTemplate> customHabits = new List<HabitTemplate>();
        foreach (var item in data)
        {
            if(item.GetType() == typeof(HabitTemplate))
            {
                customHabits.Add(item);
            }
        }
        HabitListView.ItemsSource = customHabits;
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnCreateHabitButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddCustomHabitPage());
    }

    private async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            var selectedHabit = e.Item as HabitTemplate;
            await Navigation.PushAsync(new EditCustomHabitPage(selectedHabit));
        }
        else
        {
            HabitListView.SelectedItem = null;
        }
    }
}