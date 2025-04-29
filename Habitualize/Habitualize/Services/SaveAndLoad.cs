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

namespace Habitualize.Services
{
    public class SaveAndLoad
    {
        private string _filePath = Path.Combine(FileSystem.AppDataDirectory, "Habitualize.json");

        private string _achievementsPath = Path.Combine(FileSystem.AppDataDirectory, "Achievements.json");

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
    }
}
