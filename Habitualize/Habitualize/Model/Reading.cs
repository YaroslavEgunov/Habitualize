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
        private string _pagesInBook = "";

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

        public string PagesInBook 
        {
            get => _pagesInBook;
            set
            {
                _pagesInBook = value;
            }
        }

        public string PagesRead { get; set; }
    }
}
