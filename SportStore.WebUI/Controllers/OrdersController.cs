using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using SportStore.WebUI.Models;
using System;
using System.Linq;

namespace SportStore.WebUI.Controllers
{
    public class OrdersController : Controller
    {
        UserManager<User> _userManager;
        IOrderRepository _orderRepository;
        ICartRepository _cartRepository;
        IProductOrderRepository _productOrderRepository;

        public OrdersController(UserManager<User> userManager, IOrderRepository orderRepository, ICartRepository cartRepository, IProductOrderRepository productOrderRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productOrderRepository = productOrderRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var orders = _orderRepository.GetAll().Where(n => n.UserId == _userManager.GetUserAsync(User).Result.Id).ToList();
            return View(orders);
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
        public IActionResult Create(OrderCreateViewModel orderModel)
        {
            var currUser = _userManager.GetUserAsync(User).Result;
            if (currUser.FirstName != orderModel.User.FirstName
                || currUser.LastName != orderModel.User.LastName 
                || currUser.Email != orderModel.User.Email
                || currUser.PhoneNumber != orderModel.User.PhoneNumber)
            {
                _userManager.UpdateAsync(orderModel.User);
            }
            DateTime orderDate = DateTime.Now;
            var order = new Order
            {
                OrderDate = orderDate,
                Paid = false,
                UserId = orderModel.User.Id,
            };
            _orderRepository.Add(order);
            _orderRepository.Commit();

            int orderId = _orderRepository.GetAll().First(n => n.UserId == orderModel.User.Id && n.OrderDate == orderDate).Id;

            foreach (var product in orderModel.Cart)
            {
                var productOrder = new ProductOrder
                {
                    OrderId = orderId,
                    ProductId = product.ProductId,
                    Amount = product.Amount,
                };
                _productOrderRepository.Add(productOrder);
            }
            _productOrderRepository.Commit();
            return RedirectToAction("Index");
        }
    }
}
