using Habitualize.Model;

namespace Habitualize.View;

public partial class AppMessenger : ContentPage
{
    public Friend ChatFriend { get; set; }
    public string FriendName { get; set; }
    public string FriendAvatar { get; set; }

    public AppMessenger(Friend friend)
	{
        InitializeComponent();
        ChatFriend = friend;
        FriendName = friend?.Name;
        FriendAvatar = friend?.Avatar;
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}