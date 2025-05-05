using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Habitualize.Model
{
    public class Friend
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public ICommand MessageCommand { get; set; }

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

        public Friend()
        {
            MessageCommand = new Command(() =>
            {
                Application.Current.MainPage.DisplayAlert("Message", $"Send a message to {Name}", "OK");
            });
        }
    }
}
