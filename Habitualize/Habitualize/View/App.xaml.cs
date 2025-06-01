using Habitualize.Model;
using Habitualize.Services;
using Habitualize.SignPages;
using Habitualize.View;

namespace Habitualize
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            CheckFirstLaunch();
            //MainPage = new NavigationPage(new MainPage());
            if (Preferences.Get("IsLoggedIn", false))
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                MainPage = new AppShell();
            }
            _ = PreloadUserDataAsync();
        }

        public async Task PreloadUserDataAsync()
        {
            var saveAndLoad = new SaveAndLoad();
            var userId = saveAndLoad.UserId;
            if (!string.IsNullOrEmpty(userId))
            {
                var profileTask = saveAndLoad.LoadUserBioFromFirebase(userId);
                var friendsTask = saveAndLoad.LoadFriendsFromFirebase(userId);
                var suggestedFriendsTask = saveAndLoad.LoadSuggestedFriends(userId);

                await Task.WhenAll(profileTask, friendsTask, suggestedFriendsTask);

                AppCache.UserBio = profileTask.Result;
                AppCache.Friends = friendsTask.Result;
                AppCache.SuggestedFriends = suggestedFriendsTask.Result;

                var friendProfiles = new List<Friend>();
                foreach (var friend in AppCache.Friends)
                {
                    var profile = await saveAndLoad.LoadFriendById(friend.Id);
                    if (profile != null)
                        friendProfiles.Add(profile);
                }
                AppCache.FriendProfiles = friendProfiles;
            }
        }

        private void CheckFirstLaunch()
        {
            bool isFirstLaunch = Preferences.Get("IsFirstLaunch", true);

            if (isFirstLaunch)
            {
                Preferences.Set("IsFirstLaunch", false);
            }
        }
    }
    public static class AppCache
    {
        public static string UserBio { get; set; }
        public static List<Friend> Friends { get; set; } = new();
        public static List<Friend> SuggestedFriends { get; set; } = new();
        public static List<Friend> FriendProfiles { get; set; } = new();
    }
}