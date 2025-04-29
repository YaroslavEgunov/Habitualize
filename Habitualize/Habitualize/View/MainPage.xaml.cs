using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
using Habitualize.SignPages;
using Plugin.Firebase.CloudMessaging;
using FirebaseAdmin;
using Habitualize.Services;
using Habitualize.Model;
using Habitualize.View;
using Habitualize.ViewModel;
using System.Globalization;

namespace Habitualize
{
    public partial class MainPage : ContentPage
    {
        public static SaveAndLoad SavingLoadingSystem = new SaveAndLoad();

        public static Achievements Achievements = new Achievements();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
            DynamicContent.Content = new AppMap();
            if (BindingContext is MainPageViewModel viewModel)
            {
                viewModel.ActiveTab = "Map";
            }
            NavigationPage.SetHasNavigationBar(this, false);
        }

        //Анимация при смене контента
        private async Task ChangeContentWithAnimation(Microsoft.Maui.Controls.View newContent)
        {
            if (DynamicContent.Content != null)
            {
                await DynamicContent.Content.FadeTo(0, 150, Easing.CubicInOut);
            }

            DynamicContent.Content = newContent;

            if (DynamicContent.Content != null)
            {
                DynamicContent.Content.Opacity = 0;
                await DynamicContent.Content.FadeTo(1, 150, Easing.CubicInOut);
            }
        }


        private async void OnMapButtonClicked(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
            {
                if (sender is ImageButton button)
                {
                    await button.TranslateTo(0, -10, 150, Easing.CubicInOut);
                    await button.TranslateTo(0, 0, 150, Easing.CubicInOut);
                }
                if (viewModel.ActiveTab != "Map")
                {
                    viewModel.ActiveTab = "Map";
                    await ChangeContentWithAnimation(new AppMap());
                }

            }
        }

        private async void OnProfileButtonClicked(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
            {
                if (sender is ImageButton button)
                {
                    await button.TranslateTo(0, -10, 150, Easing.CubicInOut);
                    await button.TranslateTo(0, 0, 150, Easing.CubicInOut);
                }
                if (viewModel.ActiveTab != "Profile")
                {
                    viewModel.ActiveTab = "Profile";
                    await ChangeContentWithAnimation(new AppProfile());
                }
            }   
        }

        private async void OnScheduleButtonClicked(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
            {
                if (sender is ImageButton button)
                {
                    await button.TranslateTo(0, -10, 150, Easing.CubicInOut);
                    await button.TranslateTo(0, 0, 150, Easing.CubicInOut);
                }
                if (viewModel.ActiveTab != "Schedule")
                {
                    viewModel.ActiveTab = "Schedule";
                    await ChangeContentWithAnimation(new AppSchedule());
                }
            }
        } 
        
        private async void OnChestButtonClicked(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
            {
                if (sender is ImageButton button)
                {
                    await button.TranslateTo(0, -10, 150, Easing.CubicInOut);
                    await button.TranslateTo(0, 0, 150, Easing.CubicInOut);
                }
                if (viewModel.ActiveTab != "Chest")
                {
                    viewModel.ActiveTab = "Chest";
                    await ChangeContentWithAnimation(new AppChest());
                }
            }
        }


        private async void OnSettingsButtonClicked(object sender, EventArgs e)
        {
            var authClient = new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyAO6SGqXASw8zw_YD2xqCjBBP7ZOHDDcf0",
                AuthDomain = "habitualize-249ef.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
{
            new EmailProvider()
},
                UserRepository = new FileUserRepository("Habitualize")
            });
            var signUpViewModel = new SignUpViewModel(authClient);
            if (BindingContext is MainPageViewModel viewModel)
            {
                if (sender is ImageButton button)
                {
                    await button.TranslateTo(0, -10, 150, Easing.CubicInOut);
                    await button.TranslateTo(0, 0, 150, Easing.CubicInOut);
                }
                if (viewModel.ActiveTab != "Settings")
                {
                    viewModel.ActiveTab = "Settings";
                    await ChangeContentWithAnimation(new AppSettings(signUpViewModel));
                }
            }
        }
    }

    //Конвертер для фотографий в футере
    public class TabImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var activeTab = value?.ToString();
            var tabName = parameter?.ToString();

            if (activeTab == tabName)
            {
                return $"{tabName.ToLower()}_active.png";
            }
            else
            {
                return $"{tabName.ToLower()}_unselect.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //Конвертер для подписей в футере
    public class TabVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == parameter?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
