using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Habitualize.Model;
using Habitualize.Services;

namespace Habitualize.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPlantsHabitPage : ContentPage
	{
		public Gardening CurrentPlant = new Gardening();

        public string NewTask = "";

		public AddPlantsHabitPage()
		{
            InitializeComponent();
            BindingContext = CurrentPlant;
		}

		private async void OnSelectImageClicked(object sender, EventArgs e)
		{
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Choose plant image"
            });

            if (result != null)
            {
                using (var stream = await result.OpenReadAsync())
                {
                    CurrentPlant.PlantImage.Source = ImageSource.FromStream(() => stream);
                }
            }
        }

        private async void OnAddTaskButtonClicked(object sender, EventArgs e)
        {

            string task = NewTask?.Trim();
            if (!string.IsNullOrEmpty(task))
            {
                CurrentPlant.Tasks.Add(task);
                NewTask = string.Empty; // Очистка поля ввода задачи
            }
        }

        private async void OnAddButtonClicked(object sender, EventArgs e)
        {
			var existingHabits = await MainPage.SavingLoadingSystem.LoadHabits();
			existingHabits.Add(CurrentPlant);
			await MainPage.SavingLoadingSystem.SaveHabits(existingHabits);
			var allPlants = existingHabits.OfType<Gardening>().ToList();
			await Navigation.PushAsync(new PlantsPage(allPlants));
        }
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}