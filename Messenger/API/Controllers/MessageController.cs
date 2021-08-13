using CommonClasses.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Коллекция сообщений
        /// </summary>
        private List<Message> messageList = new List<Message>();

        /// <summary>
        /// Сериализация в JSON
        /// </summary>
        private void saveToJSON(int uId)
        {
            string strMessageFilePath = Config.FilePathMessages + uId.ToString() + "_messages.json";
            List<Message> lstMessageOld = loadFromJSON(uId);
            List<Message> lstMessageToSave = lstMessageOld.Concat(messageList).ToList();
            string jsonString = JsonSerializer.Serialize(lstMessageToSave);
            System.IO.File.WriteAllText(strMessageFilePath, jsonString);
        }

        /// <summary>
        /// Десериализация из JSON
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        private List<Message> loadFromJSON(int uId)
        {
            string strMessageFilePath = Config.FilePathMessages + uId.ToString() + "_messages.json";
            var jsonBytes = System.IO.File.ReadAllBytes(strMessageFilePath);
            var obj = JsonSerializer.Deserialize(jsonBytes, typeof(List<Message>));
            List<Message> list = (List<Message>)obj;
            return list;
        }

        /// <summary>
        /// Получить все сообщения
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public List<Message> GetMessages(int uId)
        {
            messageList = loadFromJSON(uId);
            return messageList; //1 есть
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message"></param>
        [HttpPost]
        [Route("[action]")]
        public void SendMessage(Message message)
        {

            messageList.Add(message); //2 нет
            saveToJSON(message.UId);
        }
    }
}