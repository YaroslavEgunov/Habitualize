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
            //Можешь менять начальную страницу, чтобы проверить что-то
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
                // Логика для первого входа
                Preferences.Set("IsFirstLaunch", false);
            }
        }
    }
}