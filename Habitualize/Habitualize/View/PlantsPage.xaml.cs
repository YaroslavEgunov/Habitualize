using Habitualize.Model;
namespace Habitualize.View;

public partial class PlantsPage : ContentPage
{
    public PlantsPage(List<Gardening> data)
    {
        InitializeComponent();
        HabitListView.ItemsSource = data;
    }

    //async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
    //{
    //    if (e.Item == null)
    //        return;

    //    await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

    //    //Deselect Item
    //    ((ListView)sender).SelectedItem = null;
    //}

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnCreatePlantHabitButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddPlantsPage());
    }

    //private async void OnSaveHabitClicked(object sender, EventArgs e)
    //{
    //    var habit = new HabitTemplate
    //    {
    //        HabitName = HabitNameEntry.Text,
    //        HabitDescription = HabitDescriptionEntry.Text
    //    };

    //    UserData.Add(habit);
    //    await SaveAndLoad.SaveHabits(SaveAndLoad.UserData);
    //    ResultLabel.Text = "�������� ���������!";
    //}

    //private async void OnLoadHabitsClicked(object sender, EventArgs e)
    //{
    //    habits = await habitManager.LoadHabits();
    //    ResultLabel.Text = "�������� ���������!";
    //    // �������������� ��� ��� ����������� ����������� ��������
    //}
}