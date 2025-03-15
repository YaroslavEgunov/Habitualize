// Ignore Spelling: Habitualize

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    class Training : HabitTemplate
    {
        private double _weightTarget;

        private double _bodyFatPercentTarget;

        public int TotalDaysSkipped = 0;

        public double WeightTarget
        {
            get
            {
                return _weightTarget;
            }
            set
            {
                _weightTarget = value;
            }
        }

        public double BodyFatPercentTarget
        {
            get
            {
                return _bodyFatPercentTarget;
            }
            set
            {
                //minimal body fat% for human to live is 5%, less isn't healthy
                Validator.MoreThanDecimal(value, 0.05, nameof(WeightTarget));
                _bodyFatPercentTarget = value;
            }
        }


    }
}
