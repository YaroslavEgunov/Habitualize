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

    private async void OnMapButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnProfileButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AppProfile());
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