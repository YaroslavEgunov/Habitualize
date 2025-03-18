using CommunityToolkit.Mvvm.ComponentModel;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Habitualize.SignPages;
using Habitualize.View;
using Microsoft.Maui;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace Habitualize;

public partial class AppProfile : ContentPage
{
    public string Username { get; }

    public string Bio { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public AppProfile()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Username = Preferences.Get("Username", "Default Username");
        Bio = Preferences.Get("UserBio", "Enter a short biography...");

        var avatarPath = Preferences.Get("UserAvatar", string.Empty);
        if (!string.IsNullOrEmpty(avatarPath))
        {
            var imageSource = ImageSource.FromFile(avatarPath);
            var avatarImage = this.FindByName<Image>("AvatarImage");
            avatarImage.Source = imageSource;
        }

        BindingContext = this;
    }

    private async void OnAvatarTapped(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select a photo"
            });

            if (result != null)
            {
                Preferences.Set("UserAvatar", result.FullPath);

                var stream = await result.OpenReadAsync();
                var imageSource = ImageSource.FromStream(() => stream);
                var image = (Image)sender;
                image.Source = imageSource;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to select photo: {ex.Message}", "OK");
        }
    }

    private void OnBioTextChanged(object sender, TextChangedEventArgs e)
    {
        Preferences.Set("UserBio", e.NewTextValue);
    }

    private void OnBioEditorFocused(object sender, FocusEventArgs e)
    {
        BioEditor.IsReadOnly = false;
    }

    private void OnBioEditorUnfocused(object sender, FocusEventArgs e)
    {
        BioEditor.IsReadOnly = true;
        Preferences.Set("UserBio", BioEditor.Text);
    }

    private async void OnAchievementTapped(object sender, EventArgs e)
    {
        var description = (string)((TappedEventArgs)e).Parameter;
        await DisplayAlert("Achivment: ", description, "OK");
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

    private async void OnScheduleButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AppSchedule());
    }

    private async void OnMapButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}
public class Achievement
{
    public string Icon { get; set; }
    public string Description { get; set; }
}
