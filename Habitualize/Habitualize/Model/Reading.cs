using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    class Reading : HabitTemplate
    {
        public bool BookComplete = false;

        public int PagesInBook 
        { 
            get
            {
                return PagesInBook;
            }
            set
            {
                Validator.MoreThanZero(value, nameof(PagesInBook));
                PagesInBook = value;
            }
        }

        public int PagesRead = 0;
    }
}
