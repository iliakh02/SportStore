using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using SportStore.WebUI.Controllers;
using SportStore.WebUI.Interfaces;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SportStore.Tests
{
    public class ProductsControllerTests
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
        private readonly List<Product> _expectedProducts = new List<Product>
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

        private ProductsController InitializeProductsController(int productId = 1, int categoryId = 1)
        {
            var fakeUrlService = new Mock<IUrlService>();
            var fakeProductRepository = new Mock<IProductRepository>();
            var fakeCategoryRepository = new Mock<ICategoryRepository>();
            var fakeWebHostEnvironment = new Mock<IWebHostEnvironment>();

            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            fakeCategoryRepository.Setup(categoryRepository => categoryRepository.GetAll())
                .Returns(_categories);
            fakeCategoryRepository.Setup(categoryRepository => categoryRepository.GetById(It.IsAny<int>()))
                .Returns(_categories.FirstOrDefault(n => n.Id == categoryId));

            fakeUrlService.Setup(urlService => urlService.ReditectUrlForDelete(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(@"\TestController");

            fakeProductRepository.Setup(productRepository => productRepository.GetAll())
                .Returns(_expectedProducts);
            fakeProductRepository.Setup(productRepository => productRepository.GetById(It.IsAny<int>()))
                .Returns(_expectedProducts.FirstOrDefault(n => n.Id == productId));

            fakeWebHostEnvironment.Setup(webHostEnvironment => webHostEnvironment.WebRootPath)
                .Returns(path);

            var productsController = new ProductsController(fakeProductRepository.Object, fakeCategoryRepository.Object, fakeWebHostEnvironment.Object, fakeUrlService.Object);

            return productsController;
        }

        [Theory]
        [InlineData(1, "User", "Index")]
        [InlineData(2, "User", "Index")]
        [InlineData(3, "User", "Index")]
        [InlineData(4, "User", "Index")]
        [InlineData(1, "Administrator", "AdminIndex")]
        [InlineData(4, "Administrator", "AdminIndex")]
        public void IndexReturnsViewResultWithListOfProductsPerPageTest(int pageNumber, string name, string expectedView)
        {
            // Arrange
            var productsController = InitializeProductsController();
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.IsInRole(name)).Returns(true);
            //httpContext.Setup(m => m.User.FindFirst(ClaimTypes.Name)).Returns(name);

            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));

            productsController.ControllerContext = context;

            // Act
            var result = productsController.Index("", "", pageNumber);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(expectedView, viewResult.ViewName);
            Assert.Equal((pageNumber * productsController.PageSize <= _expectedProducts.Count) ?
                productsController.PageSize
                : _expectedProducts.Count - (pageNumber - 1) * productsController.PageSize
                , (viewResult?.Model as ProductsViewModel).Products.Count);
        }

        [Fact]
        public void CreateReturnsViewResultWithProductCreateViewModel()
        {
            // Arrange
            var productsController = InitializeProductsController();

            // Act
            var result = productsController.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(_categories.Count, (viewResult?.Model as ProductCreateViewModel).Categories.Count);
        }

        [Fact]
        public void CreateReturnsRedirectToActionResult()
        {
            var productsController = InitializeProductsController();
            using (var stream = new MemoryStream(new byte[] { 1, 2, 3, 4 }))
            {
                var file = new Mock<IFormFile>();

                file.Setup(f => f.FileName)
                    .Returns("test.jpg")
                    .Verifiable();
                file.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), CancellationToken.None))
                    .Callback<Stream, CancellationToken>((stream, token) =>
                    {
                        stream.CopyTo(stream);
                    }).Returns(Task.CompletedTask);
                var productCreateViewModel = new ProductCreateViewModel
                {
                    Name = "test",
                    CategoryId = 3,
                    Image = file.Object,
                };

                var result = productsController.Create(productCreateViewModel);

                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.True(viewResult.ActionName.Equals("Index"));
            }
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(1)]
        public void EditReturnsViewResultWithSelectedProduct(int productId)
        {
            // Arrange
            var productsController = InitializeProductsController(productId);

            // Act
            var result = productsController.Edit(productId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Equal(_expectedProducts[productId - 1].Id, (viewResult?.Model as ProductEditViewModel).Id);
        }

        [Theory]
        [InlineData(27)]
        [InlineData(0)]
        public void EditReturnsNotFoundForGetMethod(int productId)
        {
            // Arrange
            var productsController = InitializeProductsController(productId);

            // Act
            var result = productsController.Edit(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(9)]
        [InlineData(2)]
        public void EditReturnsRedirectToActionIndex(int productId)
        {
            // Arrange
            var productsController = InitializeProductsController(productId);

            // Act
            var result = productsController.Edit(new ProductEditViewModel
            {
                Id = 1
            });

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult?.ActionName);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(1)]
        public void EditReturnsViewsWithErrorsForPostMethod(int productId)
        {
            // Arrange
            var productsController = InitializeProductsController(productId);
            productsController.ModelState.AddModelError("", "test");

            // Act
            var result = productsController.Edit(new ProductEditViewModel
            {
 
                    Id = 1,
                    Name = "Test", 
                    Amount = 200,
                    CategoryId = 1,
                    Discount = 0.5, 
                    Description = "Test", 
                    ImagePath = "/image/test.jpg",
                    Price = 300, 
                    Producer = "Test"
            });

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(31)]
        [InlineData(-1)]
        public void DeleteReturnsNotFoundForPostMethod(int productId)
        {
            // Arrange
            var productsController = InitializeProductsController(productId);

            // Act
            var result = productsController.Delete(productId, 1, "");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(11)]
        public void DeleteReturnsRedirectToActionIndex(int productId)
        {
            // Arrange
            var productsController = InitializeProductsController(productId);

            // Act
            var result = productsController.Delete(productId, 1, "");

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(@"\TestController", viewResult?.Url);
        }
    }
}
