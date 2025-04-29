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
            AdditionalTextLabel.Text = "Добро пожаловать! Выберите категорию, чтобы начать!";
        }
        else
        {
            // Список фраз для возвращающихся пользователей
            var phrases = new List<string>
        {
            "С возвращением! Выберите категорию, чтобы продолжить!",
            "Рады снова вас видеть! Что будем делать сегодня?",
            "Приятно видеть вас снова! Выберите категорию.",
            "Добро пожаловать обратно! Давайте продолжим!",
            "Снова в деле! Выберите категорию для продолжения."
        };

            // Генерация случайного индекса
            var random = new Random();
            int randomIndex = random.Next(phrases.Count);

            // Установка случайной фразы
            AdditionalTextLabel.Text = phrases[randomIndex];
        }
    }

    private async void OnPlantsButtonClicked(object sender, EventArgs e)
    {
        var data = await SavingLoadingSystem.LoadHabits(); // Загружаем привычки
        var gardeningHabits = data.OfType<Gardening>().ToList(); // Фильтруем только привычки Gardening
        await Navigation.PushAsync(new PlantsPage(gardeningHabits)); // Передаем список привычек на вторую страницу
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