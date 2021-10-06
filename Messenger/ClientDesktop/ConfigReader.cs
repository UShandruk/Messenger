using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientDesktop
{
    /// <summary>
    /// Конфиг (парсер для Config.json)
    /// </summary>
    public class ConfigReader
    {
        /// <summary>
        /// URL-адрес API (в формате https://localhost:5001)
        /// </summary>
        public string ApiUrl;

        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public int UId;

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName;

        /// <summary>
        /// Путь к папке с сообщениями (без имени файла и с "\" в конце)
        /// </summary>
        public string FilePathMessages;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ConfigReader()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var config = new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile("Config.json").Build();
            UId = Int32.Parse(config.GetSection("UId").Value);
            UserName = config.GetSection("UserName").Value;
            ApiUrl = config.GetSection("ApiUrl").Value;
        }
    }
}