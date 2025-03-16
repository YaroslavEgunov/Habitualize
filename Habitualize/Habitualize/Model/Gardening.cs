// Ignore Spelling: Habitualize

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Habitualize.Model
{
    public class Gardening : HabitTemplate
    {
        private Image _plantImage;

        public Image PlantImage
        {
            get => _plantImage;
            set
            {
                if (value.Source.IsEmpty)
                {
                    value.Source = ImageSource.FromResource("kridisome.png");
                }
                _plantImage = value;
            }
        }

        public bool PlantIsWatered = false;

    }
}
