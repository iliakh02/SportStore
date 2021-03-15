using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportStore.Data;
using SportStore.Data.Abstract;
using SportStore.Data.Repositories;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserRepository userRepository;
        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var r = userRepository.GetAll();
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
