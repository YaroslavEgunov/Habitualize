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
            InitialData();
        }

        private async void InitialData()
        {
            var habits = new List<HabitTemplate>
            {
                new Gardening { HabitName = "Уход за растениями", HabitDescription = "Полив и удобрение"},
                new Reading { HabitName = "Чтение книг", HabitDescription = "Читать 20 страниц в день",},
                new Training { HabitName = "Фитнес", HabitDescription = "Тренироваться 3 раза в неделю"}
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

        private async void OnPlantsButtonClicked(object sender, EventArgs e)
        {
            var data = await SavingLoadingSystem.LoadHabits(); // Загружаем привычки
            var gardeningHabits = data.OfType<Gardening>().ToList(); // Фильтруем только привычки Gardening
            await Navigation.PushAsync(new PlantsPage(gardeningHabits)); // Передаем список привычек на вторую страницу
        }
    }
}
