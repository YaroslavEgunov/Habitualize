using CommunityToolkit.Maui.Views;
using Habitualize.Model;
namespace Habitualize.View;

public partial class AppAchievementPopup : Popup
{
	public AppAchievementPopup(Achievements achievements)
	{
        InitializeComponent();
        BindingContext = achievements;
    }
}