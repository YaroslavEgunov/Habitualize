using Habitualize.Model;
using Habitualize.View;
using Newtonsoft.Json;
using System.Drawing;

namespace Habitualize.View;

public partial class EditBooksPage : ContentPage
{
    private Reading _editedBook = new Reading();

    private Reading _existingBook;

    private void DeleteAtIndex(List<Reading> allBooks)
    {
        int indexToRemove = allBooks.FindIndex
                (p => p.HabitName == _existingBook.HabitName &&
                p.HabitDescription == _existingBook.HabitDescription);
        if (indexToRemove != -1)
        {
            allBooks.RemoveAt(indexToRemove);
        }
    }

    public EditBooksPage(Reading book)
    {
        InitializeComponent();
        _existingBook = JsonConvert.DeserializeObject<Reading>(JsonConvert.SerializeObject(book));
        _editedBook = book;
        BindingContext = _editedBook;
        NavigationPage.SetHasNavigationBar(this, false);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Обновление состояния BookSwitch на основе сохраненного значения
        if (_editedBook != null)
        {
            BookSwitch.IsToggled = _editedBook.BookComplete;
            BookSwitch.OnColor = Colors.LawnGreen;
        }
    }


    private async void OnConfirmButtonClicked(object sender, EventArgs e)
    {
        if (_editedBook == _existingBook)
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            await Navigation.PushAsync(new BooksPage(existingHabits.OfType<Reading>().ToList()));
        }
        else
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            var existingBooks = existingHabits.OfType<Reading>().ToList();
            if (_editedBook.PagesRead == _editedBook.PagesInBook && !_editedBook.BookComplete)
            {
                _editedBook.BookComplete = true;
            }
            DeleteAtIndex(existingBooks);
            existingBooks.Add(_editedBook);
            int j = 0;
            for (int i = 0; i < existingHabits.Count; i++)
            {
                if (existingHabits[i] is Reading)
                {
                    existingHabits[i] = existingBooks[j];
                    j++;
                }
            }
            await MainPage.CheckAchievements(existingHabits, this);
            await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
            await Navigation.PushAsync(new BooksPage(existingBooks));
        }
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
        await Navigation.PushAsync(new BooksPage(existingHabits.OfType<Reading>().ToList()));
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Delete this book", "You sure you want to delete this book?", "Yep", "Nope");
        if (result)
        {
            var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
            var existingBooks = existingHabits.OfType<Reading>().ToList();
            DeleteAtIndex(existingBooks);
            for (int i = 0; i < existingHabits.Count; i++)
            {
                if (existingHabits[i] is Reading)
                {
                    existingHabits.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < existingBooks.Count; i++)
            {
                existingHabits.Add(existingBooks[i]);
            }
            await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
            await Navigation.PushAsync(new BooksPage(existingBooks));
        }
        else
        {
            await DisplayAlert("Canceled deletion", "Fine I won't delete it, I promise", "ok...");
            return;
        }
    }
}