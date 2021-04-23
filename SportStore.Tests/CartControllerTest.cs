using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using SportStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace SportStore.Tests
{
    public class CartControllerTest
    { 
        private readonly List<Category> _categories = new List<Category>
        {
            new Category {Id = 1, Name = "Category1"},
            new Category {Id = 2, Name = "Category2"},
            new Category {Id = 3, Name = "Category3"},
            new Category {Id = 4, Name = "Category4"},
            new Category {Id = 5, Name = "Category5"},
            new Category {Id = 6, Name = "Category6"},
            new Category {Id = 7, Name = "Category7"}
        };
        private readonly List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Test", Amount = 200, CategoryId = 1, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 2, Name = "Test", Amount = 200, CategoryId = 1, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 3, Name = "Test", Amount = 200, CategoryId = 2, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 4, Name = "Test", Amount = 200, CategoryId = 3, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 5, Name = "Test", Amount = 200, CategoryId = 1, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 6, Name = "Test", Amount = 200, CategoryId = 2, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 7, Name = "Test", Amount = 200, CategoryId = 4, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 8, Name = "Test", Amount = 200, CategoryId = 1, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 9, Name = "Test", Amount = 200, CategoryId = 1, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 10, Name = "Test", Amount = 200, CategoryId = 4, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 11, Name = "Test", Amount = 200, CategoryId = 6, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 12, Name = "Test", Amount = 200, CategoryId = 7, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 13, Name = "Test", Amount = 200, CategoryId = 1, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 14, Name = "Test", Amount = 200, CategoryId = 1, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"},
            new Product { Id = 15, Name = "Test", Amount = 200, CategoryId = 5, Discount = 0.5, Description = "Test", Image = "/image/test.jpg", Price = 300, Producer = "Test"}
        };
        private readonly List<CartItem> _carts = new List<CartItem>
        {
            new CartItem {Id = 1, Amount = 5, ProductId = 15, UserId = 1},
            new CartItem {Id = 2, Amount = 1, ProductId = 1, UserId = 1},
            new CartItem {Id = 3, Amount = 2, ProductId = 4, UserId = 2},
            new CartItem {Id = 4, Amount = 1, ProductId = 5, UserId = 1},
        };

        private CartController InitializeCartController(string userId = null, int productId = 0, int cartItemId = 0)
        {
            _carts.Join(
                _products,
                c => c.ProductId,
                p => p.Id,
                (c, p) => c.Product = p).ToList();

            var fakeProductRepository = new Mock<IProductRepository>();
            fakeProductRepository.Setup(n => n.GetById(It.IsAny<int>()))
                .Returns(_products.FirstOrDefault(n => n.Id == productId));

            var fakeUserManager = new Mock<FakeManagers.FakeUserManager>();
            fakeUserManager.Setup(n => n.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns($"{userId}");

            var fakeCartRepository = new Mock<ICartRepository>();
            fakeCartRepository.Setup(n => n.GetById(It.IsAny<int>()))
                .Returns(_carts.FirstOrDefault(n => n.Id == cartItemId));
            fakeCartRepository.Setup(n => n.GetAll())
                .Returns(_carts);

            return new CartController(fakeProductRepository.Object, fakeCartRepository.Object, fakeUserManager.Object);
        }

        [Fact]
        public void IndexReturnsListOfProductsForCurrentUser()
        {
            int userId = 1;
            var cartController = InitializeCartController($"{userId}");

            var result = cartController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(_carts.Where(n => n.UserId == userId).Count(), (viewResult.Model as List<CartItem>).Count());
        }
        [Fact]
        public void IndexReturnsNotFound()
        {
            string userId = "o";
            var cartController = InitializeCartController(userId);

            var result = cartController.Index();

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void AddToCartReturnsNotFound()
        {
            string userId = "1";
            int productId = 1;
            var cartController = InitializeCartController(userId);

            var result = cartController.AddToCart(productId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void AddToCartReturnsBadRequest()
        {
            string userId = "i";
            int productId = 1;
            var cartController = InitializeCartController(userId, productId);

            var result = cartController.AddToCart(productId);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void AddToCartReturnsRedirectToUrl(int productId)
        {
            string userId = "1";
            string url = "Test";
            var cartController = InitializeCartController(userId, productId);

            var result = cartController.AddToCart(productId, url);

            var viewResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(url, viewResult.Url);
        }

        [Theory]
        [InlineData(100)]
        public void DeleteReturnsNotFound(int cartItemId)
        {
            string userId = "1";
            var cartController = InitializeCartController(userId, 0, cartItemId);

            var result = cartController.Delete(cartItemId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public void DeleteReturnsRedirectToAction(int cartItemId)
        {
            string userId = "1";
            var cartController = InitializeCartController(userId, 0, cartItemId);

            var result = cartController.Delete(cartItemId);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }


        [Theory]
        [InlineData(4)]
        public void IncreaseReturnsJsonResult(int cartItemId)
        {
            string userId = "1";
            var cartController = InitializeCartController(userId, 0, cartItemId);

            var result = cartController.Increase(cartItemId);

            Assert.IsType<JsonResult>(result);
        }
        [Theory]
        [InlineData(100)]
        public void IncreaseReturnsNull(int cartItemId)
        {
            string userId = "1";
            var cartController = InitializeCartController(userId, 0, cartItemId);

            var result = cartController.Increase(cartItemId);

            Assert.Null(result);
        }
        [Theory]
        [InlineData(4)]
        public void DecreaseReturnsJsonResult(int cartItemId)
        {
            string userId = "1";
            var cartController = InitializeCartController(userId, 0, cartItemId);

            var result = cartController.Decrease(cartItemId);

            Assert.IsType<JsonResult>(result);
        }
        [Theory]
        [InlineData(100)]
        public void DecreaseReturnsNull(int cartItemId)
        {
            string userId = "1";
            var cartController = InitializeCartController(userId, 0, cartItemId);

            var result = cartController.Decrease(cartItemId);

            Assert.Null(result);
        }
    }
}
