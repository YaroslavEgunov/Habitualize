// Ignore Spelling: Habitualize

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    public class Training : HabitTemplate
    {
        private double _weightTarget = 1;

        private double _bodyFatPercentTarget = 0.15;

        public int TotalDaysSkipped = 0;

        public double WeightTarget
        {
            get => _weightTarget;
            set
            {
                Validator.MoreThanDecimal(value, 0, nameof(WeightTarget));
                _weightTarget = value;
            }
        }

        public double BodyFatPercentTarget
        {
            get => _bodyFatPercentTarget;
            set
            {
                //minimal body fat% for human to live is 5%, less isn't healthy
                Validator.MoreThanDecimal(value, 0.05, nameof(WeightTarget));
                _bodyFatPercentTarget = value;
            }
        }


    }
}
