using Habitualize.SignPages;
using Habitualize.View;

namespace Habitualize
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            CheckFirstLaunch();
            //MainPage = new NavigationPage(new MainPage());
            if (Preferences.Get("IsLoggedIn", false))
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                MainPage = new AppShell();
            }
        }

        private void CheckFirstLaunch()
        {
            bool isFirstLaunch = Preferences.Get("IsFirstLaunch", true);

            if (isFirstLaunch)
            {
                // First launch check
                Preferences.Set("IsFirstLaunch", false);
            }
        }
    }
}