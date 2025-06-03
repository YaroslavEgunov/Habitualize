using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Habitualize.Model
{
    public class Friend:INotifyPropertyChanged
    {
        private bool _hasUnreadMessages;
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }
        public bool HasUnreadMessages
        {
            get => _hasUnreadMessages;
            set
            {
                if (_hasUnreadMessages != value)
                {
                    _hasUnreadMessages = value;
                    OnPropertyChanged(nameof(HasUnreadMessages));
                }
            }
        }
        public ICommand MessageCommand { get; set; }
        public ObservableCollection<Friend> Friends { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Friend other)
            {
                return Id == other.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
