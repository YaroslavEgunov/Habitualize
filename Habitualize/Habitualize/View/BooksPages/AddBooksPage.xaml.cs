using Habitualize.Model;
using Habitualize.View;

namespace Habitualize.View;

public partial class AddBooksPage : ContentPage
{
    public Reading CurrentBook = new Reading();

    public AddBooksPage()
	{
		InitializeComponent();
        BindingContext = CurrentBook;
        DatePick.MinimumDate = DateTime.Now;
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(BookAuthorEntry.Text) || string.IsNullOrWhiteSpace(BookAuthorEntry.Text) || string.IsNullOrWhiteSpace(BookPagesEntry.Text))
        {
            await DisplayAlert("Nuh uh!", "Please enter book name, author and amount of pages", "Sowwy...");
            return;
        }
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        MainPage.Achievements.CheckAchievements(existingHabits);
        existingHabits.Add(CurrentBook);
        await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
        var allBooks = existingHabits.OfType<Reading>().ToList();
        await Navigation.PushAsync(new BooksPage(allBooks));
    }
    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        await Navigation.PushAsync(new BooksPage(existingHabits.OfType<Reading>().ToList()));
    }
}