using CommunityToolkit.Maui.Views;
using Habitualize.Services;
using System.Collections.ObjectModel;
using Habitualize.Model;

namespace Habitualize.View;

public partial class AppFriendProfile : Popup
{
    public AppFriendProfile(string friendId)
    {
        InitializeComponent();
        LoadFriendProfile(friendId);
    }

    private async void LoadFriendProfile(string friendId)
    {
        var friend = AppCache.FriendProfiles?.FirstOrDefault(f => f.Id == friendId);

        if (friend == null)
        {
            var saveAndLoadService = new SaveAndLoad();
            friend = await saveAndLoadService.LoadFriendById(friendId);
            if (friend != null)
                AppCache.FriendProfiles.Add(friend);
        }

        if (friend != null)
        {
            var saveAndLoadService = new SaveAndLoad();
            var bio = await saveAndLoadService.LoadUserBioFromFirebase(friendId);
            var friends = await saveAndLoadService.LoadFriendsFromFirebase(friendId);

            var vm = new Friend
            {
                Id = friend.Id,
                Name = friend.Name,
                Avatar = friend.Avatar,
                Bio = bio ?? friend.Bio,
                Friends = new ObservableCollection<Friend>(friends)
            };

            BindingContext = vm;
        }
    }
}