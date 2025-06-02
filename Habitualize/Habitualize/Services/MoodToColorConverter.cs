using Habitualize.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Services
{
    class MoodToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch(value)
            {
                case "Awful":
                    return Colors.DarkRed;
                case "Bad":
                    return Colors.OrangeRed;
                case "Neutral":
                    return Colors.Gray;
                case "Good":
                    return Colors.GreenYellow;
                case "Great":
                    return Colors.Green;
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
