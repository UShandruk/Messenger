using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApi.Model;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }




        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <returns>Результат отправки</returns>
        public bool SendMessage(Message message)
        {
            bool result;
            result = true;
            return result;
        }

        /// <summary>
        /// Получить сообщения
        /// </summary>
        /// <returns>Коллекцию сообщений</returns>
        public List<Message> GetMessages()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            List<Message> messageList = new List<Message>();
            return messageList;
        }

        static HttpClient client = new HttpClient();

        //POST: api/messages
        [HttpPost]
        static async Task<Uri> SendMessageAsync(Message message)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/messages", message);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        // GET: api/User/5
        [HttpGet]
        static async Task<List<Message>> GetMessagesAsync(string path)
        {
            List<Message> messageList = new List<Message>();
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                //messageList = await response.Content.ReadAsAsync<Message>();
            }
            return messageList;
        }
    }
}