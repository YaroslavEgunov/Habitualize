using Habitualize.SignPages;
using Habitualize.View;
using Plugin.Firebase.CloudMessaging;

namespace Habitualize;

public partial class AppSettings : ContentPage
{
	public AppSettings(SignUpViewModel viewModel)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
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

    private async void OnMapButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnProfileButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AppProfile());
    }

    private async void OnScheduleButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AppSchedule());
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
        Console.WriteLine($"FCM token: {token}");
    }
}