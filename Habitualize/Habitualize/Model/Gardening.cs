using Android.Content.Res;
using Android.Icu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Habitualize.Model
{
    class Gardening : HabitTemplate
    {
        public Image PlantImage
        {
            get
            {
                return PlantImage;
            }
            set
            {
                if (value.Source.IsEmpty)
                {
                    PlantImage.Source = ImageSource.FromResource("Resources\\Images\\kridisome.png");
                }
                PlantImage = value;
            }
        }

        public bool PlantIsWatered = false;

    }
}
