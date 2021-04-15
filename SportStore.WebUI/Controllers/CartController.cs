using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStore.Data.Abstract;
using SportStore.Data.Repositories;
using SportStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        IProductRepository _productRepository;
        ICartRepository _cartRepository;
        UserManager<User> _userManager;

        public CartController(IProductRepository productRepository, ICartRepository cartRepository, UserManager<User> userManager)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            if (Int32.TryParse(_userManager.GetUserId(User), out int userId))
                return NotFound();
            var cart = _cartRepository.GetAll().Where(n => n.UserId == userId).ToList();
            return View(cart);
        }
        [HttpPost]
        public IActionResult AddToCart(int id, string returnUrl = null)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
                return NotFound();

            if (!Int32.TryParse(_userManager.GetUserId(User), out int userId))
                return BadRequest("User is incorrect.");
            var cartItem = new CartItem
            {
                ProductId = id,
                UserId = userId
            };
            _cartRepository.Add(cartItem);
            _cartRepository.Commit();
            return Redirect(returnUrl);
        }
    }
}
