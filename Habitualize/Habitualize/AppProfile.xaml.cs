namespace Habitualize;

public partial class AppProfile : ContentPage
{
	public AppProfile()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = new UserProfileViewModel();
    }

    private void OnAvatarTapped(object sender, EventArgs e)
    {
        // ���������� ������� ������� �� ������
        DisplayAlert("������", "������ ��� �����", "OK");
    }

    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AppSettings());
    }

    private async void OnMapButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}