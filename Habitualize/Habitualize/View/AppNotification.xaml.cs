namespace Habitualize.View;
using Habitualize.Model;
using Firebase.Database;
using System.Collections.ObjectModel;
using Firebase.Database.Query;
using Habitualize.Services;

public partial class AppNotification : ContentView
{
    public ObservableCollection<Notification> Notifications { get; set; }

    public AppNotification()
	{
		InitializeComponent();
        Notifications = new ObservableCollection<Notification>();
        BindingContext = this;
        LoadNotifications();
    }

    private async void LoadNotifications()
    {
        try
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            var userId = new SaveAndLoad().UserId;

            if (!string.IsNullOrEmpty(userId))
            {
                var notifications = await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("notifications")
                    .OnceAsync<Notification>();

                Notifications.Clear();
                foreach (var notification in notifications)
                {
                    Notifications.Add(notification.Object);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading notifications: {ex.Message}");
        }
    }

}