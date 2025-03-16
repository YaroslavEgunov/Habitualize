using Habitualize.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Habitualize.Model;
using Habitualize.View;
using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace Habitualize.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlantsPage : ContentPage
    {
        public PlantsPage(List<Gardening> data)
        {
            InitializeComponent();
            HabitListView.ItemsSource = data;
        }

        //async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item == null)
        //        return;

        //    await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

        //    //Deselect Item
        //    ((ListView)sender).SelectedItem = null;
        //}

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            //await Shell.Current.GoToAsync("MainPage");
            await Navigation.PopAsync();
        }

        //private async void OnSaveHabitClicked(object sender, EventArgs e)
        //{
        //    var habit = new HabitTemplate
        //    {
        //        HabitName = HabitNameEntry.Text,
        //        HabitDescription = HabitDescriptionEntry.Text
        //    };

        //    UserData.Add(habit);
        //    await SaveAndLoad.SaveHabits(SaveAndLoad.UserData);
        //    ResultLabel.Text = "Привычка сохранена!";
        //}

        //private async void OnLoadHabitsClicked(object sender, EventArgs e)
        //{
        //    habits = await habitManager.LoadHabits();
        //    ResultLabel.Text = "Привычки загружены!";
        //    // Дополнительный код для отображения загруженных привычек
        //}
    }
}
