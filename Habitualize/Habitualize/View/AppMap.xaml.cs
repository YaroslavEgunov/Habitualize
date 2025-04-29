using Habitualize.Model;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
using Habitualize.SignPages;
using Plugin.Firebase.CloudMessaging;
using FirebaseAdmin;
using Habitualize.Services;
using Habitualize.ViewModel;
using System.Globalization;

namespace Habitualize.View;

public partial class AppMap : ContentView
{
    public static SaveAndLoad SavingLoadingSystem = new SaveAndLoad();

    public AppMap()
	{
		InitializeComponent();
        UpdateAdditionalText();
    }

    private void UpdateAdditionalText()
    {
        bool isFirstLaunch = Preferences.Get("IsFirstLaunch", true);

        if (isFirstLaunch)
        {
            AdditionalTextLabel.Text = "����� ����������! �������� ���������, ����� ������!";
        }
        else
        {
            // ������ ���� ��� �������������� �������������
            var phrases = new List<string>
        {
            "� ������������! �������� ���������, ����� ����������!",
            "���� ����� ��� ������! ��� ����� ������ �������?",
            "������� ������ ��� �����! �������� ���������.",
            "����� ���������� �������! ������� ���������!",
            "����� � ����! �������� ��������� ��� �����������."
        };

            // ��������� ���������� �������
            var random = new Random();
            int randomIndex = random.Next(phrases.Count);

            // ��������� ��������� �����
            AdditionalTextLabel.Text = phrases[randomIndex];
        }
    }

    private async void OnPlantsButtonClicked(object sender, EventArgs e)
    {
        var data = await SavingLoadingSystem.LoadHabits(); // ��������� ��������
        var gardeningHabits = data.OfType<Gardening>().ToList(); // ��������� ������ �������� Gardening
        await Navigation.PushAsync(new PlantsPage(gardeningHabits)); // �������� ������ �������� �� ������ ��������
    }

    private async void OnSportButtonClicked(object sender, EventArgs e)
    {
        var data = await SavingLoadingSystem.LoadHabits();
        var trainingHabits = data.OfType<Training>().ToList();
        await Navigation.PushAsync(new SportPage(trainingHabits));
    }

    private async void OnBooksButtonClicked(object sender, EventArgs e)
    {
        var data = await SavingLoadingSystem.LoadHabits();
        var readingHabits = data.OfType<Reading>().ToList();
        await Navigation.PushAsync(new BooksPage(readingHabits));
    }

}