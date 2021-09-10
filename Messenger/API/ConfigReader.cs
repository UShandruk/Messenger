using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    /// <summary>
    /// Конфиг (парсер для Config.json)
    /// </summary>
    public class ConfigReader
    {
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
            FilePathMessages = basePath + config.GetSection("FilePathMessages").Value;
        }
    }
}