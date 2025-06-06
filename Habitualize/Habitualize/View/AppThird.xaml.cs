using Habitualize.ViewModel;

namespace Habitualize.View;

public partial class AppThird : ContentView
{
    private MainPage _mainPage;

    public AppThird(MainPage mainPage)
	{
		InitializeComponent();
        _mainPage = mainPage;
    }

    private void OnWaterTapped(object sender, EventArgs e)
    {
        _mainPage.ChangeContentWithAnimation(new AppHabitDays("Drink water", _mainPage));
    }

    private void OnExercisesTapped(object sender, EventArgs e)
    {
        _mainPage.ChangeContentWithAnimation(new AppHabitDays("Morning exercises", _mainPage));
    }

    private void OnTeethTapped(object sender, EventArgs e)
    {
        _mainPage.ChangeContentWithAnimation(new AppHabitDays("Brush teeth", _mainPage));
    }
}