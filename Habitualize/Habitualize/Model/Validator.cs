// Ignore Spelling: Habitualize Validator

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    public static class Validator
    {
        public static void TodayDateRequired(DateOnly value, string propertyName)
        {
            if (value < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new ArgumentException(
                    $"{propertyName} can't be already passed date.");
            }
        }

        public static void LessThanNCharacters(string value, int n, string propertyName)
        {
            if ((value.Length < 0 || value.Length > n) && value!=null)
            {
                throw new ArgumentException(
                    $"{propertyName} can't be more than {n} characters.");
            }
        }

        public static void MoreThanZero(int value, string propertyName)
        {
            if(value <= 0)
            {
                throw new ArgumentException(
                    $"{propertyName} can't be less than zero.");
            }
        }

        public static void MoreThanDecimal(double value, double n, string propertyName)
        {
            if(value < n)
            {
                throw new ArgumentException(
                    $"{propertyName} can't be less than {n}.");
            }
        }
    }
}
