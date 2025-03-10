using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    class Gym
    {
        public DateTime TrainingTime { get; set; }

        public int AmountOfTrainingPerWeek { get; set; }

        public int MissedTrainings { get; set; }
    }
}
