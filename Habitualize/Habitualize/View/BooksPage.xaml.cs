using Habitualize.Model;
using System.Globalization;
namespace Habitualize.View;

public partial class BooksPage : ContentPage
{

    public BooksPage(List<Reading> data)
	{
		InitializeComponent();
        HabitListView.ItemsSource = data;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnCreateReadingHabitButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddBooksPage());
    }

    private async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            var selectedBook = e.Item as Reading;
            await Navigation.PushAsync(new EditBooksPage(selectedBook));
        }
        else
        {
            HabitListView.SelectedItem = null;
        }
    }
}