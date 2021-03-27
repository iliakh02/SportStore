using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using SportStore.WebUI.Controllers;
using SportStore.WebUI.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportStore.Tests
{
    public class CategoriesControllerTests
    {
        private List<Category> _categories = new List<Category>
        {
            new Category { Id = 1, Name = "Category1" },
            new Category { Id = 2, Name = "Category2" },
            new Category { Id = 3, Name = "Category3" },
            new Category { Id = 4, Name = "Category4" },
            new Category { Id = 5, Name = "Category5" },
            new Category { Id = 6, Name = "Category6" },
            new Category { Id = 7, Name = "Category7" },
            new Category { Id = 8, Name = "Category8" },
            new Category { Id = 9, Name = "Category9" },
        };
        public CategoriesController CategoriesControllerInitializer(int categoryId = 0)
        {
            var fakeCategoryRepository = new Mock<ICategoryRepository>();
            fakeCategoryRepository.Setup(categoryRepository => categoryRepository.GetAll())
                .Returns(_categories);
            fakeCategoryRepository.Setup(categoryRepository => categoryRepository.GetById(It.IsAny<int>()))
                .Returns(_categories.FirstOrDefault(n => n.Id == categoryId));

            var categoriesController = new CategoriesController(fakeCategoryRepository.Object);

            return categoriesController;
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(500)]
        public void IndexReturnsViewResultWithListOfCategoriesPerPage(int pageNumber)
        {
            // Arrange 
            var categoriesController = CategoriesControllerInitializer();
            int countCategoriesPerPage = (pageNumber * categoriesController.PageSize <= _categories.Count()) ?
                categoriesController.PageSize
                : _categories.Count() - (pageNumber - 1) * categoriesController.PageSize;
            // Act
            var result = categoriesController.Index("", pageNumber);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal((countCategoriesPerPage < 0)? 0 : countCategoriesPerPage, (viewResult.Model as CategoriesViewModel).Categories.Count);
        }
    }
}
