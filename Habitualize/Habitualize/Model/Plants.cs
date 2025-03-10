using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    class Plants
    {
        public string PlantName { get; set; }

        //public ??? PlantImage { get; get;}

        public DateTime LastWatering { get; set; }

        public bool WateringNeeded = false;

    }
}
