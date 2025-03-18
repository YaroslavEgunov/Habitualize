using Habitualize.Model;
namespace Habitualize.View;

public partial class PlantsPage : ContentPage
{
    public PlantsPage(List<Gardening> data)
    {
        InitializeComponent();
        HabitListView.ItemsSource = data;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnCreatePlantHabitButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddPlantsPage());
    }

    private async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            // �������� ��������� �������
            var selectedPlant = e.Item as Gardening; // �������� YourHabitModel �� ���� ������ ������
            await Navigation.PushAsync(new EditPlantsPage(selectedPlant));
        }
        else
        {
            HabitListView.SelectedItem = null;
        }
    }
}