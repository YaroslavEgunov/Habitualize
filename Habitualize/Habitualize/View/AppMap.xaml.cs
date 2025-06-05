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
using Habitualize.View.CustomHabitsPages;

namespace Habitualize.View;

public partial class AppMap : ContentView
{
    private const string _lastUpdateKey = "LastUpdateDate";

    private const string _lastDailyName = "LastDailyData";

    private const string _lastDailyDescription = "LastDailyDesc";

    private void CheckAndUpdateDailyTasks()
    {
        var lastUpdate = Preferences.Get(_lastUpdateKey, DateTime.MinValue);
        var today = DateTime.Today;

        if (lastUpdate < today)
        {
            ResetDailyTasks();
            Preferences.Set(_lastUpdateKey, today);
        }
    }

    private void ResetDailyTasks()
    {
        var random = new Random();
        var chosenDaily = MainPage.Dailies[random.Next(MainPage.Dailies.Count)];
        Preferences.Set(_lastDailyName, chosenDaily.TaskName);
        Preferences.Set(_lastDailyDescription, chosenDaily.TaskDescription);
    }
    

    public DailyTasks Daily = new DailyTasks();

    public static SaveAndLoad SavingLoadingSystem = new SaveAndLoad();

    public static ProgressionData ProgressionData = new ProgressionData();

    private async void InitAsync()
    {
        UpdateAdditionalText();
        CheckAndUpdateDailyTasks();
        await SavingLoadingSystem.LoadProgress();
        Daily.TaskName = Preferences.Get(_lastDailyName, "Error");
        Daily.TaskDescription = Preferences.Get(_lastDailyDescription, "Error");
        DailiesLabel.Text = "Your task for today: " + Daily.TaskName + "\n" + Daily.TaskDescription;
        LevelLabel.Text = "Your Level: " + ProgressionSystem.Level.ToString();
        double progress = 0;

        if (ProgressionSystem.ExpForLevel > 0)
            progress = (double)ProgressionSystem.Experience / ProgressionSystem.ExpForLevel;
        // Ограничиваем значение от 0 до 1
        progress = Math.Clamp(progress, 0, 1);
        ExperienceProgressBar.Progress = progress;
    }

    public AppMap()
	{
		InitializeComponent();
        InitAsync();
    }

    private void UpdateAdditionalText()
    {
        bool isFirstLaunch = Preferences.Get("IsFirstLaunch", true);

        if (isFirstLaunch)
        {
            AdditionalTextLabel.Text = "Welcome! Choose category to begin!";
        }
        else
        {
            var phrases = new List<string>
        {
            "Welcome back! Choose a category to continue!",
            "Good to see you again! What shall we do today?",
            "Nice to have you back! Pick a category.",
            "Welcome back! Let’s get started!",
            "Back in action! Choose a category to proceed.",
            "It's dangerous to go alone! Take this.",
            "War. War never changes. But habits can!",
            "Welcome back, Commander!",
            "Ready for a new quest?",
            "Praise the sun! And pick a category.",
            "Finish him! Or start something new.",
            "You are our last hope, Commander.",
            "Welcome back, Dragonborn!",
            "Ready for the next level?",
            "May the Force be with you!",
            "I'll be back. And you are!",
            "Why so serious? Pick a category.",
            "Welcome back, Neo.",
            "You shall not pass... without choosing a category!",
            "With great power comes great responsibility.",
            "You're back? This is the beginning of something great. ",
            "Winter is coming, but habits await.",
            "Welcome back, friend!",
            "How about a legendary day? ",
            "Welcome back to Hawkins! ",
            "Just do it!",
            "Welcome back, Mr. White.",
            "You're back? This is no coincidence. ",
            "Welcome back, Sheriff.",
            "You're back! This is your day to shine.",
            "Ready for new achievements? Pick a category.",
            "Welcome back! Today is going to be great.",
            "You're with us again! Time for new victories.",
            "Good to see you again! Let’s move forward to success!"
        };
            // Random index generation
            var random = new Random();
            int randomIndex = random.Next(phrases.Count);
            // Random phrase select
            AdditionalTextLabel.Text = phrases[randomIndex];
        }
    }

    private async void OnPlantsButtonClicked(object sender, EventArgs e)
    {
        var data = await SavingLoadingSystem.LoadHabits();
        var gardeningHabits = data.OfType<Gardening>().ToList();
        await Navigation.PushAsync(new PlantsPage(gardeningHabits));
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

    private async void OnCustomHabitsButtonClicked(object sender, EventArgs e)
    {
        var data = await SavingLoadingSystem.LoadHabits();
        var customHabits = data.OfType<HabitTemplate>().ToList();
        await Navigation.PushAsync(new CustomHabitPage(customHabits));
    }
}