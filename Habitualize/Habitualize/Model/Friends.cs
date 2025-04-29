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
        public string Name { get; set; }
        public string Avatar { get; set; }
        public ICommand MessageCommand { get; set; }

        public Friend()
        {
            MessageCommand = new Command(() =>
            {
                // Логика отправки сообщения
                Application.Current.MainPage.DisplayAlert("Message", $"Send a message to {Name}", "OK");
            });
        }
    }
}
