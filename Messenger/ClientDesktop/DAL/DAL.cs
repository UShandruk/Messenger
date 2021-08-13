using CommonClasses.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ClientDesktop
{
    public class DAL
    {
        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async void SendMessageAsync(Message message)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            HttpClient httpClient = new HttpClient(handler);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(Config.ApiUrl + "/Message/SendMessage", message);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Получить все сообщения
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public static async Task<List<Message>> GetMessagesAsync(int uId)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            HttpClient httpClient = new HttpClient(handler);

            //string url = "https://localhost:5001/Message/GetMessages?uId=1";
            string url = Config.ApiUrl + "/Message/GetMessages?uId=" + uId;
            httpClient.Timeout = new TimeSpan(0, 0, 30);

            HttpResponseMessage response = await httpClient.GetAsync(url).ConfigureAwait(false);
            string jsonString = response.Content.ReadAsStringAsync().Result;

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Message> messageList = new List<Message>();
            if (jsonString.Length > 0)
            {
                messageList = JsonSerializer.Deserialize<List<Message>>(jsonString, options);
            }

            //BinaryFormatter bin = new BinaryFormatter();
            //messageList = (List<Message>)bin.Deserialize(stream);// исключение что не та последовательность

            return messageList;
        }
    }
}