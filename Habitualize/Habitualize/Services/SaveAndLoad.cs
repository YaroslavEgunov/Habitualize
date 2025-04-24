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

        private async Task SaveToFile(string jsonContent)
        {
            await File.WriteAllTextAsync(_filePath, jsonContent);
        }

        private async Task<string> LoadFromFile()
        {
            return await File.ReadAllTextAsync(_filePath);
        }

        public async Task SaveHabits(List<HabitTemplate> habits)
        {
            string json = JsonConvert.SerializeObject(habits, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            await SaveToFile(json);
        }

        public async Task<List<HabitTemplate>> LoadHabits()
        {
            if (!File.Exists(_filePath) || new FileInfo(_filePath).Length == 0)
            {
                await SaveToFile("[]");
                return new List<HabitTemplate>();
            }
            string json = await LoadFromFile();
            return JsonConvert.DeserializeObject<List<HabitTemplate>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}
