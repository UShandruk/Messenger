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
using CommonClasses;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private ConfigReader configReader = new ConfigReader();

        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }

        ///// <summary>
        ///// Коллекция сообщений
        ///// </summary>
        private List<Message> messageList = new List<Message>();

        /// <summary>
        /// Получить все сообщения
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public List<Message> GetMessages(int uId)
        {
            string filePath = configReader.FilePathMessages + uId.ToString() + "_messages.json";
            messageList = JsonHelper.LoadFromJSON(uId, filePath); //loadFromJSON(uId);
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
            int uId = message.UId;
            string filePath = configReader.FilePathMessages + uId.ToString() + "_messages.json";
            messageList.Add(message); //2 нет
            //saveToJSON(message.UId);
            JsonHelper.SaveToJSON(uId, filePath, messageList);
        }
    }
}