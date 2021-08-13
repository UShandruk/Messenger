using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientDesktop
{
    public static class Config
    {
        /// <summary>
        /// URL-адрес API (в формате https://localhost:5001)
        /// </summary>
        //public static string ApiUrl = "https://localhost:44347"; // API в дебаге
        public static string ApiUrl = "https://localhost:5001"; // API из exe

        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public static int UId = 1;

        /// <summary>
        /// Имя пользователя (берется системное)
        /// </summary>
        public static string UserName = Environment.UserName;
    }
}
