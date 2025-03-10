using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    class Reading
    {
        public string BookName { get; set; }

        public int AmountOfPages { get; set; }

        public bool BookComplete = false;
    }
}
