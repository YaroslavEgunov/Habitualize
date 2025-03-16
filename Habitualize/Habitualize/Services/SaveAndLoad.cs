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
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Habitualize.Services
{
    public class SaveAndLoad
    {
        private async Task SaveToFile(string fileName, string jsonContent)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            await File.WriteAllTextAsync(filePath, jsonContent);
        }

        private async Task<string> LoadFromFile(string fileName)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            return await File.ReadAllTextAsync(filePath);
        }
        public async Task SaveHabits(List<HabitTemplate> habits)
        {
            string json = JsonSerializer.Serialize(habits);
            await SaveToFile("Habitualize.json", json);
        }

        public async Task<List<HabitTemplate>> LoadHabits()
        {
            string json = await LoadFromFile("Habitualize.json");
            return JsonSerializer.Deserialize<List<HabitTemplate>>(json);
        }
    }
}
