using Habitualize.SignPages;
using Habitualize.View;
using Plugin.Firebase.CloudMessaging;
using Habitualize.Model;
using Android.Content;
using Android.App;
using Application = Microsoft.Maui.Controls.Application;
using Android.Appwidget;

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
            await Clipboard.SetTextAsync(token);
            Console.WriteLine($"FCM token: {token}");
            await Application.Current.MainPage.DisplayAlert("Success", "FCM added to clipboard.", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "FCM token wasn't recieved.", "OK");
        }
    }

    private async void OnSaveFirebaseClicked(object sender, EventArgs e)
    {
        List<HabitTemplate> habits = await MainPage.SavingLoadingSystem.LoadHabits();
        var achievements = MainPage.Achievements;
        await MainPage.SavingLoadingSystem.SaveInFirebase(habits, achievements.AchievementsList);
        await Application.Current.MainPage.DisplayAlert("Success", "Data saved!", "Yuppie");
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
        await MainPage.SavingLoadingSystem.SaveHabits(habits);
#if ANDROID
        SaveHabitsForWidget(habits);
        UpdateWidget();
#endif
        await Application.Current.MainPage.DisplayAlert("Success", $"Loaded this: {text}", "Yuppie");
    }

#if ANDROID
    public static void SaveHabitsForWidget(IEnumerable<HabitTemplate> habits)
    {
        var context = Android.App.Application.Context;
        var prefs = context.GetSharedPreferences("habitualize", FileCreationMode.Private);
        var editor = prefs.Edit();

        for (int i = 0; i < 7; i++)
        {
            var date = DateTime.Today.AddDays(i);
            var habitsForDate = habits
                .Where(h => h.RepeatSchedule.Date == date && !h.IsComplete)
                .Select(h => h.HabitName)
                .ToList();

            var value = habitsForDate.Count > 0
                ? string.Join("\n", habitsForDate)
                : "Nothing";

            editor.PutString($"habits_{date:yyyyMMdd}", value);
        }
        editor.Apply();
    }
#endif
#if ANDROID
    public static void UpdateWidget()
    {
        var context = Android.App.Application.Context;
        var intent = new Intent(context, typeof(Habitualize.Platforms.Android.HabitWidgetProvider));
        intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
        int[] ids = AppWidgetManager.GetInstance(context)
            .GetAppWidgetIds(new ComponentName(context, Java.Lang.Class.FromType(typeof(Habitualize.Platforms.Android.HabitWidgetProvider)).Name));
        intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, ids);
        context.SendBroadcast(intent);
    }
#endif

}