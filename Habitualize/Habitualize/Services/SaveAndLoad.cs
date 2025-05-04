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

namespace Habitualize.Services
{
    public class SaveAndLoad
    {
        private string _filePath = Path.Combine(FileSystem.AppDataDirectory, "Habitualize.json");

        private string _achievementsPath = Path.Combine(FileSystem.AppDataDirectory, "Achievements.json");

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

        private async Task SaveToFile(string filePath,string jsonContent)
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
            foreach(var achievement in MainPage.Achievements.AchievementsList)
            {
                if(achievement.Unlocked)
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
            foreach(var symbol in json)
            {
                switch(symbol)
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
                await SaveToFile(_filePath,"[]");
                return new List<HabitTemplate>();
            }
            LoadAchievements();
            string json = await LoadFromFile(_filePath);
            return JsonConvert.DeserializeObject<List<HabitTemplate>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public async Task SaveInFirebase(List<HabitTemplate> habits, List<AchievementsTemplate> achievements)
        {
            var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
            // Получите текущего пользователя
            var uid = _authClient.User.Uid;

            if (uid != null)
            {
                // Сохраняем список привычек
                await firebase
                    .Child("user")
                    .Child(uid)
                    .Child("habits")
                    .PutAsync(habits);
                // Сохраняем список достижений
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
            byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
            string base64Image = Convert.ToBase64String(imageBytes);

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

        public async Task<List<Friend>> LoadSuggestedFriends(List<Friend> currentFriends)
        {
            try
            {
                var firebase = new FirebaseClient("https://habitualize-249ef-default-rtdb.europe-west1.firebasedatabase.app/");
                var allUsers = await firebase
                    .Child("user")
                    .OnceAsync<Dictionary<string, object>>(); // Загружаем всех пользователей как словарь

                // Получаем список идентификаторов уже существующих друзей
                var existingFriendIds = new HashSet<string>(currentFriends.Select(f => f.Id));

                // Добавляем текущего пользователя в список исключений
                if (!string.IsNullOrEmpty(_authClient.User?.Uid))
                {
                    existingFriendIds.Add(_authClient.User.Uid);
                }

                var suggestedFriends = new List<Friend>();

                foreach (var user in allUsers)
                {
                    // Пропускаем текущего пользователя и уже существующих друзей
                    if (existingFriendIds.Contains(user.Key))
                        continue;

                    // Загружаем фотографию пользователя
                    var base64Photo = await LoadUserPhotoFromFirebase(user.Key);

                    // Создаем объект Friend
                    var friend = new Friend
                    {
                        Id = user.Key, // Используем ключ как Id
                        Name = $"User {user.Key}", // Временно используем Id как имя
                        Avatar = base64Photo // Сохраняем Base64-строку в Avatar
                    };

                    // Проверяем, есть ли уже такой друг в списке предложений
                    if (!suggestedFriends.Any(f => f.Id == friend.Id))
                    {
                        suggestedFriends.Add(friend);
                    }

                    // Прерываем, если уже выбрано 3 уникальных пользователя
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

                // Проверяем, существует ли друг с таким Id
                var existingFriends = await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("friends")
                    .OnceAsync<Friend>();

                if (existingFriends.Any(f => f.Object.Id == friend.Id))
                {
                    Console.WriteLine($"Friend with Id {friend.Id} already exists for user {userId}.");
                    return; // Друг уже существует, не добавляем его снова
                }

                // Добавляем друга под его Id
                await firebase
                    .Child("user")
                    .Child(userId)
                    .Child("friends")
                    .Child(friend.Id) // Используем Id друга как ключ
                    .PutAsync(friend);

                Console.WriteLine($"Friend {friend.Id} saved to Firebase for user {userId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddFriendToFirebase: {ex.Message}");
                throw;
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

                // Возвращаем список друзей
                return friends.Select(f => f.Object).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadFriendsFromFirebase: {ex.Message}");
                return new List<Friend>();
            }
        }


    }
    public class FriendComparer : IEqualityComparer<Friend>
    {
        public bool Equals(Friend x, Friend y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id;
        }

        public int GetHashCode(Friend obj)
        {
            return obj.Id.GetHashCode();
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
                    return ImageSource.FromFile("bob_avatar.png"); // Заглушка
                }
            }
            return ImageSource.FromFile("bob_avatar.png"); // Заглушка
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
