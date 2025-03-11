namespace Habitualize.SignPages;

public partial class SignUpView : ContentPage
{
	public SignUpView(SignUpViewModel viewModel)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = viewModel;
    }
}