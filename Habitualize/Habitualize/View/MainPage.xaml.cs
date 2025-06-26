using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
using Habitualize.SignPages;
using Microsoft.Maui.Controls;
using Plugin.Firebase.CloudMessaging;
using FirebaseAdmin;
using Microsoft.Maui.Storage;
using Habitualize.Services;
using Habitualize.Model;
using Habitualize.View;
using Habitualize.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Habitualize
{
    public partial class MainPage : ContentPage
    {
        public static SaveAndLoad SavingLoadingSystem = new SaveAndLoad();

        public static Achievements Achievements = new Achievements();

        public static MoodDiary Diary = new MoodDiary();

        public static List<DailyTasks> Dailies = new List<DailyTasks>
        {
            new DailyTasks
            {
                TaskName = "Hydrate!",
                TaskDescription = "Water your plant",
                Condition = () => true == true
            },
            new DailyTasks
            {
                TaskName = "Read!",
                TaskDescription = "Read 10 pages",
                Condition = () => 10 >= 10
            },
            new DailyTasks
            {
                TaskName = "Do stuff!",
                TaskDescription = "Complete custom habit",
                Condition = () => true == true
            }
        };

        public static async Task CheckAchievements(List<HabitTemplate> habits, Page currentPage)
        {
            foreach (var achievement in Achievements.AchievementsList)
            {
                bool isCompleted = achievement.IsAchievementComplete(achievement, habits);
                if (isCompleted)
                {
                    if (!achievement.Unlocked)
                    {
                        achievement.Unlocked = true;
                        await currentPage.DisplayAlert("Achievement complete!", $"You completed achievement {achievement.Name}!", "Awesome");
                    }
                }
            }
        }

        public MainPage()
        {
            InitializeComponent();
            var viewModel = new MainPageViewModel();
            bool tutorialShown = Preferences.Get("TutorialShown", false);
            viewModel.IsTutorialActive = true;
            if (!tutorialShown)
            {
                viewModel.IsTutorialActive = true;
                Preferences.Set("TutorialShown", true);
            }
            else
            {
                viewModel.IsTutorialActive = false;
            }
            BindingContext = viewModel;
            DynamicContent.Content = new AppMap();
            viewModel.ActiveTab = "Map";
            NavigationPage.SetHasNavigationBar(this, false);
            viewModel.TutorialContentRequested += OnTutorialContentRequested;
        }

        private void OnTutorialContentRequested(string contentKey)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (contentKey == "Settings")
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
                    await ChangeContentWithAnimation(new AppSettings(signUpViewModel));
                }
                else if (contentKey == "Map")
                {
                    await ChangeContentWithAnimation(new AppMap());
                }
                else if (contentKey == "Profile")
                {
                    await ChangeContentWithAnimation(new AppProfile());
                }
                else if (contentKey == "Schedule")
                {
                    await ChangeContentWithAnimation(new AppSchedule());
                }
                else if (contentKey == "Third")
                {
                    await ChangeContentWithAnimation(new AppThird(this));
                }
            });
        }

        //Animation
        public async Task ChangeContentWithAnimation(Microsoft.Maui.Controls.View newContent)
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

        private async void OnNotificationButtonClicked(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
            {
                if (sender is ImageButton button)
                {
                    await button.TranslateTo(0, -10, 150, Easing.CubicInOut);
                    await button.TranslateTo(0, 0, 150, Easing.CubicInOut);
                }
                if (viewModel.ActiveTab != "Notification")
                {
                    viewModel.ActiveTab = "Notification";
                    await ChangeContentWithAnimation(new AppNotification());
                }
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

        private async void OnThirdButtonClicked(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
            {
                if (sender is ImageButton button)
                {
                    await button.TranslateTo(0, -10, 150, Easing.CubicInOut);
                    await button.TranslateTo(0, 0, 150, Easing.CubicInOut);
                }
                if (viewModel.ActiveTab != "Third")
                {
                    viewModel.ActiveTab = "Third";
                    await ChangeContentWithAnimation(new AppThird(this));
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

    //Converter for images in footer
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

    //Converter for text in footer
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
