using CommunityToolkit.Maui.Views;
using Habitualize.Model;

namespace Habitualize.View;

public partial class AppFriendsPopup : Popup
{
	public AppFriendsPopup(object friends)
	{
        InitializeComponent();
        BindingContext = new { Friends = friends };
    }
}