using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Config
    {
        public Config()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var config = new ConfigurationBuilder().SetBasePath(basePath)
            .AddJsonFile("Config.json").Build();
            FilePathMessages = basePath + config.GetSection("FilePathMessages").Value;
        }

        /// <summary>
        /// Путь к папке с сообщениями (без имени файла и с "\" в конце)
        /// </summary>
        public static string FilePathMessages = @"C:\MessengerApiData\";
    }
}
