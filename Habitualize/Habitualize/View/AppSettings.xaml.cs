using Habitualize.SignPages;
using Habitualize.View;

namespace Habitualize;

public partial class AppSettings : ContentPage
{
	public AppSettings(SignUpViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        NavigationPage.SetHasNavigationBar(this, false);
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
}