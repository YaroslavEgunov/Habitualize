using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Habitualize.Model;

namespace Habitualize.Services
{
    class AchievementCompletionToImage : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool unlocked)
            {
                return unlocked ? parameter : "achievement_locked.png";
            }
            return "achievement_locked.png";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
