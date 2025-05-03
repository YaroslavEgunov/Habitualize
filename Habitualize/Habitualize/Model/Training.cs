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
        private string _targetWeight = "";

        private string _currentWeight = "";

        private bool _trainingComplete = false;

        public string TargetWeight
        {
            get => _targetWeight;
            set
            {
                _targetWeight = value;
            }
        }

        public string CurrentWeight
        {
            get => _currentWeight;
            set
            {
                _currentWeight = value;
            }
        }

        public bool TrainingComplete
        {
            get => _trainingComplete;
            set
            {
                OnPropertyChanged(nameof(TrainingComplete));
                _trainingComplete = value;
            }
        }

        public string Type = "Training";
    }
}
