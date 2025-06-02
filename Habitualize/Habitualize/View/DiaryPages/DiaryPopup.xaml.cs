using CommunityToolkit.Maui.Views;
using Habitualize.Model;
namespace Habitualize.View.DiaryPages;

public partial class DiaryPopup : Popup
{
	public DiaryPopup(MoodDiary entry)
	{
		InitializeComponent();
		BindingContext = entry;
    }
}