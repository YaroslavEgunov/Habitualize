using Habitualize.Model;
using System.Runtime.CompilerServices;

namespace Habitualize.View.DiaryPages;

public partial class AddDiaryEntry : ContentPage
{
	private MoodDiary _currentEntry = new MoodDiary();

	private List<string> _moods = new List<string>
	{
		"Awful",
		"Bad",
		"Neutral",
		"Good",
		"Great"
	};

	public AddDiaryEntry()
	{
		InitializeComponent();
		BindingContext = _currentEntry;
        MoodPicker.ItemsSource = _moods;
    }

	private async void OnBackButtonClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
		var existingEntries = await MainPage.SavingLoadingSystem.LoadDiary();
		existingEntries.Add(_currentEntry);
		await MainPage.SavingLoadingSystem.SaveDiary(existingEntries);
        await Navigation.PushAsync(new DiaryPage(existingEntries));
    }
}