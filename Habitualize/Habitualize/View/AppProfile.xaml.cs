using CommunityToolkit.Mvvm.ComponentModel;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Habitualize.Model;
using Habitualize.SignPages;
using Habitualize.ViewModel;
using Microsoft.Maui;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;


namespace Habitualize.View;

public partial class AppProfile : ContentView
{
    private string _avatarImageSource;

    public ObservableCollection<Friend> Friends { get; set; }

    public string Username { get; }

    public string Bio { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public string AvatarImageSource
    {
        get => _avatarImageSource;
        set
        {
            _avatarImageSource = value;
            Preferences.Set("UserAvatar", value);
            OnPropertyChanged(nameof(AvatarImageSource));
        }
    }

    public AppProfile()
    {
        InitializeComponent();

        Username = Preferences.Get("Username", "Default Username");
        Bio = Preferences.Get("UserBio", "Enter a short biography...");
        AvatarImageSource = Preferences.Get("UserAvatar", string.Empty);

        Friends = new ObservableCollection<Friend>
        {
            new Friend { Name = "Alice", Avatar = "alice_avatar.png" },
            new Friend { Name = "Bob", Avatar = "bob_avatar.png" },
            new Friend { Name = "Charlie", Avatar = "charlie_avatar.png" }
        };

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
                AvatarImageSource = result.FullPath; // Обновляем источник изображения

                var stream = await result.OpenReadAsync();
                var imageSource = ImageSource.FromStream(() => stream);
                var avatarImage = this.FindByName<Image>("AvatarImage");
                avatarImage.Source = imageSource; // Обновляем отображение
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





}
