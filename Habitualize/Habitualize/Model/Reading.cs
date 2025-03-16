// Ignore Spelling: Habitualize

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize.Model
{
    public class Reading : HabitTemplate
    {
        private int _pagesInBook = 1;

        public bool BookComplete = false;

        public int PagesInBook 
        {
            get => _pagesInBook;
            set
            {
                Validator.MoreThanZero(value, nameof(PagesInBook));
                _pagesInBook = value;
            }
        }

        public int PagesRead = 0;
    }
}
