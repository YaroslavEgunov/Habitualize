// Ignore Spelling: Habitualize

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Habitualize.Services;
using System.Text.Json.Serialization;

namespace Habitualize.Model
{
    public class Gardening : HabitTemplate
    {
        private bool _plantIsWatered = false;

        public bool PlantIsWatered
        {
            get => _plantIsWatered;
            set
            {
                _plantIsWatered = value;
                OnPropertyChanged(nameof(PlantIsWatered));
            }
        }

        public string Type = "Gardening";

    }
}
