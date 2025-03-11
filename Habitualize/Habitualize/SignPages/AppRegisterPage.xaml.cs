namespace Habitualize.SignPages;

public partial class AppRegisterPage : ContentPage
{
	public AppRegisterPage()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }
    //private async void OnSignInButtonClicked(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new SignUpView());
    //}

    //private async void OnSignUpButtonClicked(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new SignInView());
    //}
}