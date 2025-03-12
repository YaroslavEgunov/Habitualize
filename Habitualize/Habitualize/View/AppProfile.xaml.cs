using CommunityToolkit.Mvvm.ComponentModel;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Habitualize.SignPages;
using Habitualize.View;

namespace Habitualize;

public partial class AppProfile : ContentPage
{
    public string Username { get; }
    public AppProfile()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Username = Preferences.Get("Username", "Default Username");
        BindingContext = this;
    }

    private void OnAvatarTapped(object sender, EventArgs e)
    {
        // Обработчик события нажатия на аватар
        DisplayAlert("Аватар", "Аватар был нажат", "OK");
    }

    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        var authClient = new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = "AIzaSyAO6SGqXASw8zw_YD2xqCjBBP7ZOHDDcf0",
            AuthDomain = "habitualize-249ef.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
        {
            new EmailProvider()
        },
            UserRepository = new FileUserRepository("Habitualize")
        });
        await Navigation.PushAsync(new AppSettings(new SignUpViewModel(authClient)));
    }

    private async void OnMapButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}
