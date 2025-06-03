// Ignore Spelling: Deserialize

using Habitualize.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Management;
using FirebaseAdmin.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using FirebaseAdmin;
using Habitualize.SignPages;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Google.Apis.Util;
using System.Diagnostics;
using System.Globalization;
using System.Collections.ObjectModel;
using SkiaSharp;

namespace Habitualize.Services
{
    public class SaveAndLoad
    {
        private string _filePath = Path.Combine(FileSystem.AppDataDirectory, "Habitualize.json");

        private string _achievementsPath = Path.Combine(FileSystem.AppDataDirectory, "Achievements.json");

        private string _diaryPath = Path.Combine(FileSystem.AppDataDirectory, "Diary.json");

        //For db connection
        private readonly FirebaseAuthClient _authClient = new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = "AIzaSyAO6SGqXASw8zw_YD2xqCjBBP7ZOHDDcf0",
            AuthDomain = "habitualize-249ef.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                },
            UserRepository = new FileUserRepository("Habitualize")
        });

        private async Task SaveToFile(string filePath, string jsonContent)
        {
            await File.WriteAllTextAsync(filePath, jsonContent);
        }

        private async Task<string> LoadFromFile(string filePath)
        {
            return await File.ReadAllTextAsync(filePath);
        }

        private async void SaveAchievements(List<HabitTemplate> habits)
        {
            string completionData = "";
            foreach (var achievement in MainPage.Achievements.AchievementsList)
            {
                if (achievement.Unlocked)
                {
                    completionData += "1";
                }
                else
                {
                    completionData += "0";
                }
            }
            string achievementsJson = JsonConvert.SerializeObject(completionData);
            await SaveToFile(_achievementsPath, achievementsJson);
        }

        private async void LoadAchievements()
        {
            if (!File.Exists(_achievementsPath) || new FileInfo(_achievementsPath).Length == 0)
            {
                await SaveToFile(_achievementsPath, "[]");
                return;
            }
            string json = await LoadFromFile(_achievementsPath);
            json.ToCharArray();
            int i = 0;
            //Store achievements data as binary
            foreach (var symbol in json)
            {
                switch (symbol)
                {
                    case '1':
                        MainPage.Achievements.AchievementsList[i].Unlocked = true;
                        i++;
                        break;
                    case '0':
                        MainPage.Achievements.AchievementsList[i].Unlocked = false;
                        i++;
                        break;
                    default:
                        break;
                }
            }
        }

        public string UserId => _authClient.User?.Uid;

        public async Task SaveHabits(List<HabitTemplate> habits)
        {
            SaveAchievements(habits);
            //Settings required for polymorphism
            string json = JsonConvert.SerializeObject(habits, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            await SaveToFile(_filePath, json);
        }

        public async Task<List<HabitTemplate>> LoadHabits()
        {
            if (!File.Exists(_filePath) || new FileInfo(_filePath).Length == 0)
            {
                await SaveToFile(_filePath, "[]");
                return new List<HabitTemplate>();
            }
            LoadAchievements();
            string json = await LoadFromFile(_filePath);
            return JsonConvert.DeserializeObject<List<HabitTemplate>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public async Task SaveDiary(List<MoodDiary> diary)
        {
            string json = JsonConvert.SerializeObject(diary);
            await SaveToFile(_diaryPath, json);
        }

        public async Task<List<MoodDiary>> LoadDiary()
        {
            if (!File.Exists(_diaryPath) || new FileInfo(_diaryPath).Length == 0)
            {
                await SaveToFile(_diaryPath, "[]");
                return new List<MoodDiary>();
            }
            string json = await LoadFromFile(_diaryPath);
            return JsonConvert.DeserializeObject<List<MoodDiary>>(json);
        }

        public async Task SaveInFirebase(List<HabitTemplate> habits, List<AchievementsTemplate> achievements)
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            var uid = _authClient.User.Uid;

            if (uid != null)
            {
                await firebase
                    .Child("user")
                    .Child(uid)
                    .Child("habits")
                    .PutAsync(habits);
                await firebase
                    .Child("user")
                    .Child(uid)
                    .Child("achievements")
                    .PutAsync(achievements);
            }
        }

        public async Task<List<HabitTemplate>> LoadHabitsFromFirebase()
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            var uid = _authClient.User.Uid;
            if (uid != null)
            {
                var customHabits = await firebase
                    .Child("user")
                    .Child(uid)
                    .Child("habits")
                    .OnceAsListAsync<HabitTemplate>();
                customHabits = customHabits.Where(habit => habit.Object.Type == "HabitTemplate").ToList();
                var gardeningHabits = await firebase
                    .Child("user")
                    .Child(uid)
                    .Child("habits")
                    .OnceAsListAsync<Gardening>();
                gardeningHabits = gardeningHabits.Where(habit => habit.Object.Type == "Gardening").ToList();
                var readingHabits = await firebase
                    .Child("user")
                    .Child(uid)
                    .Child("habits")
                    .OnceAsListAsync<Reading>();
                readingHabits = readingHabits.Where(habit => habit.Object.Type == "Reading").ToList();
                var trainingHabits = await firebase
                    .Child("user")
                    .Child(uid)
                    .Child("habits")
                    .OnceAsListAsync<Training>();
                trainingHabits = trainingHabits.Where(habit => habit.Object.Type == "Training").ToList();
                List<HabitTemplate> allHabits = new List<HabitTemplate>();
                allHabits.AddRange(customHabits.Select(habit => habit.Object));
                allHabits.AddRange(gardeningHabits.Select(habit => habit.Object));
                allHabits.AddRange(readingHabits.Select(habit => habit.Object));
                allHabits.AddRange(trainingHabits.Select(habit => habit.Object));
                return allHabits;
            }
            return null;
        }

        public async Task<List<AchievementsTemplate>> LoadAchievementsFromFirebase()
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            var uid = _authClient.User.Uid;
            if (uid != null)
            {
                var achievements = await firebase
                    .Child("user")
                    .Child(uid)
                    .Child("achievements")
                    .OnceAsListAsync<AchievementsTemplate>();

                return achievements.Select(a => a.Object).ToList();
            }
            return null;
        }

        public async Task SaveUserPhotoToFirebase(string userId, string imagePath)
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");

            byte[] originalBytes = await File.ReadAllBytesAsync(imagePath);
            using var inputStream = new MemoryStream(originalBytes);
            using var original = SKBitmap.Decode(inputStream);

            int targetSize = 100;
            int width, height;
            if (original.Width > original.Height)
            {
                width = targetSize;
                height = original.Height * targetSize / original.Width;
            }
            else
            {
                height = targetSize;
                width = original.Width * targetSize / original.Height;
            }

            using var resized = original.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium);
            using var image = SKImage.FromBitmap(resized);

            using var ms = new MemoryStream();
            image.Encode(SKEncodedImageFormat.Jpeg, 70).SaveTo(ms);

            string base64Image = Convert.ToBase64String(ms.ToArray());

            if (!string.IsNullOrEmpty(userId))
            {
                var jsonObject = new { ImageData = base64Image };
                await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("profilePhoto")
                    .PutAsync(jsonObject);
            }
        }

        public async Task<string> LoadUserPhotoFromFirebase(string userId)
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            if (!string.IsNullOrEmpty(userId))
            {
                var jsonObject = await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("profilePhoto")
                    .OnceSingleAsync<Dictionary<string, string>>();

                if (jsonObject != null && jsonObject.ContainsKey("ImageData"))
                {
                    return jsonObject["ImageData"];
                }
            }
            return null;
        }

        public async Task<List<Friend>> LoadSuggestedFriends(string userId)
        {
            try
            {
                var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");

                var currentFriends = await LoadFriendsFromFirebase(userId);
                var existingFriendIds = new HashSet<string>(currentFriends.Select(f => f.Id));

                if (!string.IsNullOrEmpty(userId))
                {
                    existingFriendIds.Add(userId);
                }

                var allUsers = await firebase
                    .Child("user")
                    .OnceAsync<Dictionary<string, object>>();

                var suggestedFriends = new List<Friend>();

                foreach (var user in allUsers)
                {
                    if (existingFriendIds.Contains(user.Key))
                        continue;

                    var base64Photo = await LoadUserPhotoFromFirebase(user.Key);

                    // Загружаем имя пользователя по Id
                    var userName = await LoadUserNameFromFirebase(user.Key);

                    var friend = new Friend
                    {
                        Id = user.Key,
                        Name = userName ?? $"User {user.Key}",
                        Avatar = base64Photo
                    };

                    if (!suggestedFriends.Any(f => f.Id == friend.Id))
                    {
                        suggestedFriends.Add(friend);
                    }

                    if (suggestedFriends.Count == 3)
                        break;
                }

                return suggestedFriends;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadSuggestedFriends: {ex.Message}");
                return new List<Friend>();
            }
        }


        public async Task AddFriendToFirebase(Friend friend, string userId)
        {
            try
            {
                var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");

                var existingFriends = await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("friends")
                    .OnceAsync<Friend>();

                if (existingFriends.Any(f => f.Object.Id == friend.Id))
                {
                    Console.WriteLine($"Friend with Id {friend.Id} already exists for user {userId}.");
                    return;
                }

                await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("friends")
                    .Child(friend.Id)
                    .PutAsync(friend);

                Console.WriteLine($"Friend {friend.Id} saved to Firebase for user {userId}.");

                var currentUserName = await LoadUserNameFromFirebase(userId);
                var currentUserAvatar = await LoadUserPhotoFromFirebase(userId);

                var currentUser = new Friend
                {
                    Id = userId,
                    Name = currentUserName ?? userId,
                    Avatar = currentUserAvatar
                };

                await firebase
                    .Child("user")
                    .Child(friend.Id)
                    .Child("friends")
                    .Child(userId)
                    .PutAsync(currentUser);

                Console.WriteLine($"User {userId} added to friends of {friend.Id}.");

                var notification = new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = $"{currentUserName ?? "User"} add you as friend.",
                    Timestamp = DateTime.UtcNow
                };

                await firebase
                    .Child("user")
                    .Child(friend.Id)
                    .Child("notifications")
                    .Child(notification.Id)
                    .PutAsync(notification);

                Console.WriteLine($"Notification added for user {friend.Id}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddFriendToFirebase: {ex.Message}");
                throw;
            }
        }

        public async Task RemoveFriendFromFirebase(string userId, string friendId)
        {
            try
            {
                var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");

                await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("friends")
                    .Child(friendId)
                    .DeleteAsync();

                await firebase
                    .Child("user")
                    .Child(friendId)
                    .Child("friends")
                    .Child(userId)
                    .DeleteAsync();

                Console.WriteLine($"Friend {friendId} delete {userId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }


        public async Task SaveUserNameToFirebase(string userId, string userName)
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userName))
            {
                await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("name")
                    .PutAsync(userName);
            }
        }

        public async Task<string> LoadUserNameFromFirebase(string userId)
        {
            try
            {
                var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
                var userName = await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("name")
                    .OnceSingleAsync<string>();
                return userName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadUserNameFromFirebase: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Friend>> LoadFriendsFromFirebase(string userId)
        {
            try
            {
                var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
                var friends = await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("friends")
                    .OnceAsync<Friend>();

                var result = new List<Friend>();

                foreach (var f in friends)
                {
                    var friend = f.Object;
                    var userName = await LoadUserNameFromFirebase(friend.Id);
                    if (!string.IsNullOrEmpty(userName))
                        friend.Name = userName;

                    result.Add(friend);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadFriendsFromFirebase: {ex.Message}");
                return new List<Friend>();
            }
        }


        public async Task SaveUserBioToFirebase(string userId, string bio)
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            if (!string.IsNullOrEmpty(userId))
            {
                var bioObject = new { text = bio ?? string.Empty };
                await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("bio")
                    .PutAsync(bioObject);
            }
        }

        public async Task<string> LoadUserBioFromFirebase(string userId)
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            if (!string.IsNullOrEmpty(userId))
            {
                var bioObj = await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("bio")
                    .OnceSingleAsync<Dictionary<string, string>>();

                if (bioObj != null && bioObj.TryGetValue("text", out var bioText))
                    return bioText;
            }
            return null;
        }

        public async Task<Friend> LoadFriendById(string friendId)
        {
            try
            {
                var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
                if (string.IsNullOrEmpty(friendId))
                    return null;

                var name = await firebase
                    .Child("user")
                    .Child(friendId)
                    .Child("name")
                    .OnceSingleAsync<string>();

                var avatar = await LoadUserPhotoFromFirebase(friendId);

                string bio = null;
                var bioObj = await firebase
                    .Child("user")
                    .Child(friendId)
                    .Child("bio")
                    .OnceSingleAsync<Dictionary<string, string>>();
                if (bioObj != null && bioObj.TryGetValue("text", out var bioText))
                    bio = bioText;

                var friendsList = await LoadFriendsFromFirebase(friendId);
                var friends = friendsList != null ? new ObservableCollection<Friend>(friendsList) : new ObservableCollection<Friend>();

                var friend = new Friend
                {
                    Id = friendId,
                    Name = name ?? $"User {friendId}",
                    Avatar = avatar,
                    Bio = bio,
                    Friends = friends
                };

                return friend;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadFriendById: {ex.Message}");
                return null;
            }
        }

        public async Task SaveMessageToFirebase(Message message, string userId1, string userId2)
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            string chatId = GetChatId(userId1, userId2);
            var result = await firebase
                .Child("chats")
                .Child(chatId)
                .PostAsync(message);


            message.Id = result.Key;

            await firebase
                .Child("chats")
                .Child(chatId)
                .Child(result.Key)
                .Child("Id")
                .PutAsync(JsonConvert.SerializeObject(result.Key));
        }

        public async Task<List<Message>> LoadMessagesFromFirebase(string userId1, string userId2, DateTime? since = null)
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            string chatId = GetChatId(userId1, userId2);
            var messages = await firebase
                .Child("chats")
                .Child(chatId)
                .OnceAsync<Message>();

            var result = messages.Select(m => m.Object)
                .Where(m => since == null || m.Timestamp >= since.Value)
                .OrderBy(m => m.Timestamp)
                .ToList();

            return result;
        }

        public async Task MarkMessageAsReadAsync(Message message, string userId1, string userId2)
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            string chatId = GetChatId(userId1, userId2);

            if (!string.IsNullOrEmpty(message.Id))
            {
                var cleanId = message.Id.Trim('"');

                await firebase
                    .Child("chats")
                    .Child(chatId)
                    .Child(message.Id)
                    .Child("IsRead")
                    .PutAsync(true);

                message.IsRead = true;
            }
        }


        private string GetChatId(string userId1, string userId2)
        {
            return string.CompareOrdinal(userId1, userId2) < 0
                ? $"{userId1}_{userId2}"
                : $"{userId2}_{userId1}";
        }
    }

    public class Base64ToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string base64 && !string.IsNullOrEmpty(base64))
            {
                try
                {
                    byte[] imageBytes = System.Convert.FromBase64String(base64);
                    return ImageSource.FromStream(() => new MemoryStream(imageBytes));
                }
                catch
                {
                    return ImageSource.FromFile("bob_avatar.png"); 
                }
            }
            return ImageSource.FromFile("bob_avatar.png"); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
