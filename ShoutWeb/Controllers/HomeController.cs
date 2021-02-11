using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoutWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShoutWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Word word)
        {
            Word wordResult = new Word();

            using (var client = new HttpClient())
            {
                var content = new StringContent("{\"value\":" + "\"" + word.Value +"\"}", Encoding.UTF8, "application/json");
                // var content = new StringContent("{\"value\": " + textLowcase + "}", Encoding.UTF8, "application/json");
                var responseTask = client.PostAsync("https://localhost:44392/api/uppercase", content);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    wordResult.Value = readTask.Result;
                }
            }

            ViewData["resp"] = wordResult.Value;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
