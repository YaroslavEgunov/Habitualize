using CommunityToolkit.Maui.Views;
using Habitualize.Model;
using Habitualize.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Habitualize.View;

public partial class AppFriendsPopup : Popup
{
    public ObservableCollection<Friend> Friends { get; set; }
    public ICommand RemoveFriendCommand { get; }

    public AppFriendsPopup(object friends)
	{
        InitializeComponent();
        Friends = (ObservableCollection<Friend>?)friends;
        RemoveFriendCommand = new Command<Friend>(RemoveFriend);
        BindingContext = this;
    }
    private async void RemoveFriend(Friend friend)
    {
        if (friend == null) return;
        Friends.Remove(friend);
        var saveAndLoad = new SaveAndLoad();
        var userId = saveAndLoad.UserId;
        if (!string.IsNullOrEmpty(userId))
            await saveAndLoad.RemoveFriendFromFirebase(userId, friend.Id);
    }

}