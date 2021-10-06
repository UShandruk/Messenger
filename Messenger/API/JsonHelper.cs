using CommonClasses.Model;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace API
{
    /// <summary>
    /// Класс для работы с JSON
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Сериализация в JSON
        /// </summary>
        public static void SaveToJSON(int uId, string filePath, List<Message> messages)
        {
            List <Message> lstMessageOld = LoadFromJSON(uId, filePath);
            List<Message> lstMessageToSave = lstMessageOld.Concat(messages).ToList();
            string jsonString = JsonSerializer.Serialize(lstMessageToSave);
            System.IO.File.WriteAllText(filePath, jsonString);
        }

        /// <summary>
        /// Десериализация из JSON
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public static List<Message> LoadFromJSON(int uId, string filePath)
        {
            List<Message> messages = new List<Message>();
            // Десериализация выполняется только если файл существует
            if (System.IO.File.Exists(filePath))
            {
                var jsonBytes = System.IO.File.ReadAllBytes(filePath);
                var obj = JsonSerializer.Deserialize(jsonBytes, typeof(List<Message>));
                messages = (List<Message>)obj;
            }
            return messages;
        }
    }
}
