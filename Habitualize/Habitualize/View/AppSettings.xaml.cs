using Habitualize.SignPages;
using Habitualize.View;
using Plugin.Firebase.CloudMessaging;
using Habitualize.Model;

namespace Habitualize;

public partial class AppSettings : ContentView
{
	public AppSettings(SignUpViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    private async void OnPrivacyPolicyClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://drive.google.com/file/d/1VIKkbDjYDUNsH8zoGkf29vU9N56H9-5e/view?usp=sharing"));
    }

    private async void OnTermsAndConditionsClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://drive.google.com/file/d/14zHOxYc0s_7LxC62gaEheVa2AY0pUUxu/view?usp=sharing"));
    }


    private void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        Preferences.Remove("IsLoggedIn");
        Application.Current.MainPage = new AppShell();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
        var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();

        if (!string.IsNullOrEmpty(token))
        {
            await Clipboard.SetTextAsync(token); // Копируем токен в буфер обмена
            Console.WriteLine($"FCM token: {token}");
            await Application.Current.MainPage.DisplayAlert("Успех", "FCM токен скопирован в буфер обмена.", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось получить FCM токен.", "OK");
        }
    }

    private async void OnSaveFirebaseClicked(object sender, EventArgs e)
    {
        List<HabitTemplate> habits = await MainPage.SavingLoadingSystem.LoadHabits();
        var achievements = MainPage.Achievements;
        await MainPage.SavingLoadingSystem.SaveInFirebase(habits, achievements.AchievementsList);
    }

    private async void OnLoadFirebaseClicked(object sender, EventArgs e)
    {
        List<HabitTemplate> habits = await MainPage.SavingLoadingSystem.LoadHabitsFromFirebase();
        var achievementsData = await MainPage.SavingLoadingSystem.LoadAchievementsFromFirebase();
        MainPage.Achievements.AchievementsList = achievementsData;
        var text = "";
        foreach(var habit in habits)
        {
            text += $"\n{habit.HabitName},";
        }
        await Application.Current.MainPage.DisplayAlert("Success", $"Loaded this: {text}", "Yuppie");
    }
}