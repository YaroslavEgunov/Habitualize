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
using CommunityToolkit.Maui.Views;
using Habitualize.Services;
using System.Windows.Input;
using Firebase.Database;
using Firebase.Database.Query;


namespace Habitualize.View;

public partial class AppProfile : ContentView
{
    private ImageSource _avatarImageSource;

    private bool IsBase64String(string base64)
    {
        if (string.IsNullOrEmpty(base64))
            return false;
        base64 = base64.Trim();
        if (base64.Length % 4 != 0)
            return false;
        return System.Text.RegularExpressions.Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,2}$", System.Text.RegularExpressions.RegexOptions.None);
    }

    private async void LoadAvatarFromFirebase()
    {
        try
        {
            var saveAndLoadService = new SaveAndLoad();
            var userId = saveAndLoadService.UserId;

            if (!string.IsNullOrEmpty(userId))
            {
                var base64Image = await saveAndLoadService.LoadUserPhotoFromFirebase(userId);

                if (!string.IsNullOrEmpty(base64Image))
                {
                    base64Image = base64Image.Replace("\r", "").Replace("\n", "").Trim();

                    if (IsBase64String(base64Image))
                    {
                        byte[] imageBytes = Convert.FromBase64String(base64Image);
                        AvatarImageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                        Preferences.Set("UserAvatar", base64Image);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "The image data is not a valid Base64 string.", "OK");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load photo: {ex.Message}", "OK");
        }
    }

    public ObservableCollection<Friend> SuggestedFriends { get; set; }

    public ICommand AddFriendCommand { get; }

    public ObservableCollection<Friend> Friends { get; set; }

    public string Username { get; }

    public string Bio { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public ImageSource AvatarImageSource
    {
        get => _avatarImageSource;
        set
        {
            _avatarImageSource = value;
            OnPropertyChanged(nameof(AvatarImageSource));
        }
    }

    public AppProfile()
    {
        InitializeComponent();

        Username = Preferences.Get("Username", "Default Username");
        Bio = Preferences.Get("UserBio", "Enter a short biography...");

        AvatarImageSource = "bob_avatar.png";
        var savedAvatar = Preferences.Get("UserAvatar", string.Empty);
        if (!string.IsNullOrEmpty(savedAvatar) && IsBase64String(savedAvatar))
        {
            byte[] imageBytes = Convert.FromBase64String(savedAvatar);
            AvatarImageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
        else
        {
            LoadAvatarFromFirebase();
        }

        Friends = new ObservableCollection<Friend>();
        SuggestedFriends = new ObservableCollection<Friend>();

        AddFriendCommand = new Command<Friend>(AddFriend);
        Console.WriteLine("AddFriendCommand initialized.");

        LoadFriends();
        LoadSuggestedFriends();

        BindingContext = this;
    }

    private async void LoadSuggestedFriends()
    {
        try
        {
            var saveAndLoadService = new SaveAndLoad();
            var userId = saveAndLoadService.UserId;

            if (!string.IsNullOrEmpty(userId))
            {
                var randomUsers = await saveAndLoadService.LoadSuggestedFriends(userId);

                SuggestedFriends.Clear();
                foreach (var user in randomUsers)
                {
                    SuggestedFriends.Add(user);
                }
            }
            else
            {
                Console.WriteLine("User ID is null or empty.");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load suggested friends: {ex.Message}", "OK");
        }
    }


    private async void LoadFriends()
    {
        try
        {
            var saveAndLoadService = new SaveAndLoad();
            var userId = saveAndLoadService.UserId;
            if (!string.IsNullOrEmpty(userId))
            {
                var friends = await saveAndLoadService.LoadFriendsFromFirebase(userId);
                Friends.Clear();
                foreach (var friend in friends)
                {
                    Friends.Add(friend);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LoadFriends: {ex.Message}");
        }
    }

    private async void AddFriend(Friend friend)
    {
        try
        {
            if (friend == null)
            {
                Console.WriteLine("Friend is null.");
                return;
            }

            Console.WriteLine($"Attempting to add friend: {friend.Id}");

            if (!Friends.Any(f => f.Id == friend.Id))
            {
                Console.WriteLine("Friend not in Friends list. Adding...");
                Friends.Add(friend);
                SuggestedFriends.Remove(friend);

                Console.WriteLine("Saving friend to Firebase...");
                var saveAndLoadService = new SaveAndLoad();
                var userId = saveAndLoadService.UserId;
                if (!string.IsNullOrEmpty(userId))
                {
                    await saveAndLoadService.AddFriendToFirebase(friend, userId);
                    Console.WriteLine($"Friend {friend.Id} added successfully for user {userId}.");
                }
                else
                {
                    Console.WriteLine("User ID is null or empty.");
                }
            }
            else
            {
                Console.WriteLine("Friend already exists in Friends list.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddFriend: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to add friend: {ex.Message}", "OK");
        }
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
                AvatarImageSource = result.FullPath; 

                var stream = await result.OpenReadAsync();
                var imageSource = ImageSource.FromStream(() => stream);
                var avatarImage = this.FindByName<Image>("AvatarImage");
                avatarImage.Source = imageSource; 

                var saveAndLoadService = new SaveAndLoad();

                var userId = saveAndLoadService.UserId; 

                if (!string.IsNullOrEmpty(userId))
                {
                    await saveAndLoadService.SaveUserPhotoToFirebase(userId, result.FullPath);
                    byte[] imageBytes = File.ReadAllBytes(result.FullPath);
                    string base64Image = Convert.ToBase64String(imageBytes);
                    Preferences.Set("UserAvatar", base64Image);

                    await Application.Current.MainPage.DisplayAlert("Success", "Photo saved to Firebase successfully.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "User ID is not available.", "OK");
                }
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

    private void OnSeeAllTapped(object sender, EventArgs e)
    {
        if (BindingContext is AppProfile viewModel && viewModel.Friends != null)
        {
            var friendsPopup = new AppFriendsPopup(viewModel.Friends);
            if (Application.Current.MainPage is Microsoft.Maui.Controls.Page currentPage)
            {
                currentPage.ShowPopup(friendsPopup);
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Error", "Unable to display the popup.", "OK");
            }
        }
        else
        {
            Application.Current.MainPage.DisplayAlert("Error", "Friends list is not available.", "OK");
        }
    }
}
