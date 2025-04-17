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
        private int _pagesInBook = 0;

        private bool _bookComplete = false;

        public bool BookComplete 
        {
            get => _bookComplete;
            set
            {
                _bookComplete = value;
                OnPropertyChanged(nameof(BookComplete));
            }
        }

        public int PagesInBook 
        {
            get => _pagesInBook;
            set
            {
                _pagesInBook = value;
            }
        }

        public int PagesRead { get; set; }
    }
}
