using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.Utilities.Extensions
{
    public static class JsonExtensions
    {
        /// <summary>
        /// json序列化,默认设置ReferenceLoopHandling.Ignore,Formatting.Indented
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="jsonSerializerSettings"></param>
        /// <returns></returns>
        public static string ToJson(object obj, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (obj == null)
                return null;
            return JsonConvert.SerializeObject(obj,
                jsonSerializerSettings ?? new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                });
        }

        public static async Task<string> SerializeAsync(object obj, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return await Task.Run<string>(() =>
              {
                  return ToJson(obj, jsonSerializerSettings);
              });
        }

        public static T Deserialize<T>(string json)
        {

            return JsonConvert.DeserializeObject<T>(json);
        }
        public static async Task<T> DeserializeAsync<T>(string json)
        {
            return await Task.Run(() =>
                {
                    return JsonConvert.DeserializeObject<T>(json);
                });
        }

        public static List<T> DeserializeToList<T>(string json)
        {
            return Deserialize<List<T>>(json);
        }


        public static void SerializeToFile(object obj, string filePath, JsonSerializerSettings jsonSerializerSettings = null)
        {
            string s = ToJson(obj, jsonSerializerSettings);
            //var path = Path.GetDirectoryName(filePath);
            //if (!Directory.Exists(path))
            //    Directory.CreateDirectory(path);
            File.WriteAllText(filePath, s, Encoding.UTF8);
        }

        public static async Task SerializeToFileAsync(object obj, string filePath, JsonSerializerSettings jsonSerializerSettings = null)
        {
            string s = await SerializeAsync(obj, jsonSerializerSettings);
            await Task.Run(() =>
              {

                  //var path = Path.GetDirectoryName(filePath);
                  //if (!Directory.Exists(path))
                  //    Directory.CreateDirectory(path);
                  File.WriteAllText(filePath, s, Encoding.UTF8);
              });
        }


        public static T DeserializeFromFile<T>(string filePath)
        {
            string s = File.ReadAllText(filePath, Encoding.UTF8);

            return Deserialize<T>(s);
        }


        public static async Task<T> DeserializeFromFileAsync<T>(string filePath)
        {
            return await Task.Run(() =>
            {
                string s = File.ReadAllText(filePath, Encoding.UTF8);

                return Deserialize<T>(s);
            });
        }

        public static List<T> DeserializeFromFileToList<T>(string filePath)
        {
            string s = File.ReadAllText(filePath, Encoding.UTF8);

            return DeserializeToList<T>(s);
        }
    }
}
