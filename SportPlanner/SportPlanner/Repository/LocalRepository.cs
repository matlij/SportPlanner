using Newtonsoft.Json;
using SportPlanner.Repository.Interfaces;
using System;
using System.IO;

namespace SportPlanner.Repository
{
    public class LocalRepository<T> : ILocalRepository<T>
    {
        public T GetEntity(string fileName)
        {
            var filePath = GetFilePath(fileName);
            if (!File.Exists(filePath))
            {
                return default;
            }
            var data = File.ReadAllText(filePath);
            return string.IsNullOrEmpty(data)
                ? default :
                JsonConvert.DeserializeObject<T>(data);
        }

        public bool UpsertEntity(string fileName, T entity)
        {
            var filePath = GetFilePath(fileName);
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            var data = JsonConvert.SerializeObject(entity);
            File.WriteAllText(filePath, data);
            return true;
        }

        public bool DeleteEntity(string fileName)
        {
            var filePath = GetFilePath(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return true;
        }

        private static string GetFilePath(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
        }
    }
}
