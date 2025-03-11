namespace Habitualize.View;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }
}