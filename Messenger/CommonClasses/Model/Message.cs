using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClasses.Model
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public int UId { get; set; }

        /// <summary>
        /// Дата и время отправки сообщения
        /// </summary>
        public DateTime Datetime { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="uId">Уникальный идентификатор пользователя</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="text">Текст сообщения</param>
        public Message(int uId, string userName, string text)
        {
            UId = uId;
            Datetime = DateTime.Now;
            Username = userName;
            Text = text;
        }

        public override String ToString()
        {
            String stringMessage = this.Datetime.ToString("") + " " + this.Username + " " + Environment.NewLine + this.Text + Environment.NewLine;
            return stringMessage;
        }
    }
}