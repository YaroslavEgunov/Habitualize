using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
using Habitualize.SignPages;
using Plugin.Firebase.CloudMessaging;
using FirebaseAdmin;
using Habitualize.Services;
using Habitualize.Model;
using Habitualize.View;

namespace Habitualize
{
    public partial class MainPage : ContentPage
    {
        public static SaveAndLoad SavingLoadingSystem = new SaveAndLoad();

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            if (Achievments.FirstTimeLoad)
            {
                InitialData();
                Achievments.FirstTimeLoad = false;
            }
        }

        private async void InitialData()
        {
            var habits = new List<HabitTemplate>
            {
                new Gardening { HabitName = "Уход за растениями", HabitDescription = "Поливать"},
                new Reading { HabitName = "Чтение книг", HabitDescription = "Читать",},
                new Training { HabitName = "Фитнес", HabitDescription = "Тренироваться"}
            };
            await SavingLoadingSystem.SaveHabits(habits);
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
            await Navigation.PushAsync(new AppSettings(new SignUpViewModel(authClient)));
        }

        private async void OnProfileButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppProfile());
        }

        private async void OnScheduleButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppSchedule());
        }

        private async void OnPlantsButtonClicked(object sender, EventArgs e)
        {
            var data = await SavingLoadingSystem.LoadHabits(); // Загружаем привычки
            var gardeningHabits = data.OfType<Gardening>().ToList(); // Фильтруем только привычки Gardening
            await Navigation.PushAsync(new PlantsPage(gardeningHabits)); // Передаем список привычек на вторую страницу
        }

        private async void OnSportButtonClicked(object sender, EventArgs e)
        {
            var data = await SavingLoadingSystem.LoadHabits();
            var trainingHabits = data.OfType<Training>().ToList();
            await Navigation.PushAsync(new SportPage(trainingHabits));
        }

        private async void OnBooksButtonClicked(object sender, EventArgs e)
        {
            var data = await SavingLoadingSystem.LoadHabits();
            var readingHabits = data.OfType<Reading>().ToList();
            await Navigation.PushAsync(new BooksPage(readingHabits));
        }
    }
}
