using Habitualize.Model;
using Habitualize.View;
using Habitualize.Services;

namespace Habitualize.View;

public partial class SportPage : ContentPage
{
	public SportPage(List<Training> data)
	{
		InitializeComponent();
        HabitListView.ItemsSource = data;
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void OnCreateTrainingHabitButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddSportPage());
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            var selectedTraining = e.Item as Training;
            await Navigation.PushAsync(new EditSportPage(selectedTraining));
        }
        else
        {
            HabitListView.SelectedItem = null;
        }
    }
}