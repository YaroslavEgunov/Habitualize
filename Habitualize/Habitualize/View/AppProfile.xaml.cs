using CommunityToolkit.Mvvm.ComponentModel;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Habitualize.SignPages;
using Habitualize.ViewModel;
using Microsoft.Maui;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace Habitualize.View;

public partial class AppProfile : ContentView
{
    public string Username { get; }

    public string Bio { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public AppProfile()
    {
        InitializeComponent();

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
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to select photo: {ex.Message}", "OK");
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
        await Application.Current.MainPage.DisplayAlert("Achivment: ", description, "OK");
    }
}
public class Achievement
{
    public string Icon { get; set; }
    public string Description { get; set; }
}
