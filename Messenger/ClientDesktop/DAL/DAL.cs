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
        private ConfigReader configReader;

        /// <summary>
        /// Конструктор
        /// </summary>
        public DAL()
        {
            configReader = new ConfigReader();
        }

        /// <summary>
        /// Получить Http-клиент с отключенной проверкой ssl-сертификата
        /// </summary>
        /// <returns></returns>
        private static HttpClient GetHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient httpClient = new HttpClient(handler);
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            return httpClient;
        }

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async void SendMessageAsync(Message message)
        {
            HttpClient httpClient = GetHttpClient();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(configReader.ApiUrl + "/Message/SendMessage", message);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Получить все сообщения
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public async Task<List<Message>> GetMessagesAsync(int uId)
        {
            string url = configReader.ApiUrl + "/Message/GetMessages?uId=" + uId;
            HttpClient httpClient = GetHttpClient();

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