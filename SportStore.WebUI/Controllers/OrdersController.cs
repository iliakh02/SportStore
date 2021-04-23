using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Controllers
{
    public class OrdersController : Controller
    {
        UserManager<User> _userManager;
        IOrderRepository _orderRepository;
        ICartRepository _cartRepository;

        public OrdersController(UserManager<User> userManager, IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var user = _userManager.GetUserAsync(User);
            var orderCreateModel = new OrderCreateViewModel
            {
                User = user.Result,
                Cart = _cartRepository.GetAll().Where(n => n.UserId == user.Result.Id).ToList()
            };
            return View(orderCreateModel);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            return View();
        }
    }
}
