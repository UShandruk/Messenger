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
<<<<<<< HEAD
        private static HttpClientHandler handler;
        private static HttpClient GethttpClient()
=======
        /// <summary>
        /// Получить Http-клиент с отключенной проверкой ssl-сертификата
        /// </summary>
        /// <returns></returns>
        private static HttpClient _getHttpClient()
>>>>>>> 49f56984634bf99be74a9902a1eb8c7c15568d5a
        {
            handler = new HttpClientHandler();
            HttpClient httpClient = new HttpClient(handler);
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            return httpClient;
        }

<<<<<<< HEAD
=======
            HttpClient httpClient = new HttpClient(handler);
            return httpClient;
        }

>>>>>>> 49f56984634bf99be74a9902a1eb8c7c15568d5a
        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async void SendMessageAsync(Message message)
        {
<<<<<<< HEAD
            HttpClient httpClient = GethttpClient();
               HttpResponseMessage response = await httpClient.PostAsJsonAsync(Config.ApiUrl + "/Message/SendMessage", message);
=======
            HttpClient httpClient = _getHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(Config.ApiUrl + "/Message/SendMessage", message);
>>>>>>> 49f56984634bf99be74a9902a1eb8c7c15568d5a
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Получить все сообщения
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public static async Task<List<Message>> GetMessagesAsync(int uId)
        {
<<<<<<< HEAD
=======
            HttpClient httpClient = _getHttpClient();

>>>>>>> 49f56984634bf99be74a9902a1eb8c7c15568d5a
            //string url = "https://localhost:5001/Message/GetMessages?uId=1";
            string url = Config.ApiUrl + "/Message/GetMessages?uId=" + uId;
            HttpClient httpClient = GethttpClient();

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