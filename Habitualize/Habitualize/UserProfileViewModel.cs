using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habitualize
{
    public class UserProfileViewModel
    {
        public ObservableCollection<Friend> Friends { get; set; }
        public ObservableCollection<Achievement> Achievements { get; set; }

        public UserProfileViewModel()
        {
            // Инициализация данных
            Friends = new ObservableCollection<Friend>
        {
            new Friend { Name = "Friend 1", Avatar = "Resources/Images/catisimo.jpg" },
            new Friend { Name = "Friend 2", Avatar = "Resources/Images/catisimo.jpg" }
        };

            Achievements = new ObservableCollection<Achievement>
        {
            new Achievement { Icon = "Resources/Images/catisimo.jpg" },
            new Achievement { Icon = "Resources/Images/attentionpro.jpg" },
            new Achievement { Icon = "Resources/Images/kridisome.jpg" }
        };
        }
    }

    public class Friend
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
    }

    public class Achievement
    {
        public string Icon { get; set; }
    }
}
