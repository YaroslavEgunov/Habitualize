using CommunityToolkit.Maui.Views;
using Habitualize.Model;
namespace Habitualize.View.DiaryPages;

public partial class DiaryPage : ContentPage
{
	public DiaryPage(List<MoodDiary> diaryEntries)
	{
		InitializeComponent();
		DiaryEntries.ItemsSource = diaryEntries;
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        var item = e.Item as MoodDiary;
        var diaryPopup = new DiaryPopup(item);
        if (Application.Current.MainPage is Page currentPage)
        {
            currentPage.ShowPopup(diaryPopup);
        }
    }

    private async void OnAddEntryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddDiaryEntry());
    }
}