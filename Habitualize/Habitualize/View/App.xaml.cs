using Habitualize.SignPages;
using Habitualize.View;

namespace Habitualize
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //Можешь менять начальную страницу, чтобы проверить что-то
            //MainPage = new NavigationPage(new MainPage());

            MainPage = new AppShell();
        }
    }
}