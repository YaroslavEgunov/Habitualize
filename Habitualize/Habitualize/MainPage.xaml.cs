namespace Habitualize
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void OnSettingsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppSettings());
        }

        private async void OnProfileButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppProfile());
        }
    }

}
