using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
using Habitualize.SignPages;
using Plugin.Maui.Calendar.Models;

namespace Habitualize.View;

public partial class AppSchedule : ContentPage
{
    public EventCollection Events { get; set; }

    public AppSchedule()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        Events = new EventCollection
        {
            [DateTime.Now] = new List<EventModel>
            {
                new() { Name = "Cool event1", Description = "This is Cool event1's description!" },
                new() { Name = "Cool event2", Description = "This is Cool event2's description!" }
            },
            // 5 days from today
            [DateTime.Now.AddDays(5)] = new List<EventModel>
            {
                new() { Name = "Cool event3", Description = "This is Cool event3's description!" },
                new() { Name = "Cool event4", Description = "This is Cool event4's description!" }
            },
            // 3 days ago
            [DateTime.Now.AddDays(-3)] = new List<EventModel>
            {
                new() { Name = "Cool event5", Description = "This is Cool event5's description!" }
            },
            // custom date
            [new DateTime(2025, 3, 16)] = new List<EventModel>
            {
                new() { Name = "Cool event6", Description = "This is Cool event6's description!" }
            }
        };
        BindingContext = this;

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

    private async void OnMapButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnProfileButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AppProfile());
    }
}

internal class EventModel
{
    public string Name { get; set; }

    public string Description { get; set; }
}