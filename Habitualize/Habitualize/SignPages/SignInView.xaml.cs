namespace Habitualize.SignPages;

public partial class SignInView : ContentPage
{
	public SignInView(SignInViewModel viewModel)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = viewModel;
    }
}