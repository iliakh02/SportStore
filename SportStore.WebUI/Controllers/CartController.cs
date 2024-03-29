﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using System;
using System.Linq;

namespace SportStore.WebUI.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<User> _userManager;

        public CartController(IProductRepository productRepository, ICartRepository cartRepository, UserManager<User> userManager)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        [Route("Cart")]
        public IActionResult Index()
        {
            if (!Int32.TryParse(_userManager.GetUserId(User), out int userId))
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

            var cartItems = _cartRepository.GetAll().Where(n => n.UserId == userId).ToList();
            var currentCartItem = cartItems?.FirstOrDefault(n => n.ProductId == product.Id);

            if(currentCartItem != null)
            {
                var updateCartItem = new CartItem
                {
                    Id = currentCartItem.Id,
                    ProductId = currentCartItem.ProductId,
                    UserId = currentCartItem.UserId,
                    Amount = currentCartItem.Amount + 1
                };
                _cartRepository.Update(updateCartItem);
                _cartRepository.Commit();
                return Redirect(returnUrl);
            }

            var cartItem = new CartItem
            {
                ProductId = id,
                UserId = userId,
                Amount = 1
            };
            _cartRepository.Add(cartItem);
            _cartRepository.Commit();
            return Redirect(returnUrl);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _cartRepository.GetById(id);
            if (product == null)
                return NotFound();

            _cartRepository.Delete(product);
            _cartRepository.Commit();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Increase(int id)
        {
            var item = UpdateAmount(id, true);

            if (item == null)
                return null;

            return Json(new
            {
                id = item.Id,
                amount = item.Amount,
                price = item.Product.Price * item.Amount,
                diffInTotalPrice = item.Product.Price
            });
        }

        [HttpPost]
        public JsonResult Decrease(int id)
        {
            var item = UpdateAmount(id, false);

            if (item == null)
                return null;
            
            return Json(new {
                id = item.Id,
                amount = item.Amount,
                price = item.Product.Price * item.Amount,
                diffInTotalPrice = -item.Product.Price
            });
        }

        private CartItem UpdateAmount(int id, bool increase)
        {
            CartItem item = _cartRepository.GetAll().FirstOrDefault(n => n.Id == id);
            if (item == null)
                return null;

            if (increase)
                item.Amount++;
            else
                item.Amount--;

            _cartRepository.Update(item);
            _cartRepository.Commit();

            return item;
        }
    }
}
