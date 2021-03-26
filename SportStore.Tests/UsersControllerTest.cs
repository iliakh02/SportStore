using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStore.Models.Entities;
using SportStore.Tests.FakeManagers;
using SportStore.WebUI.Areas.Admin.Controllers;
using SportStore.WebUI.Areas.Admin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SportStore.Tests
{
    public class UsersControllerTest
    {
        List<User> expectedUsers = new List<User>
            {
                new User { Id = 1, Email = "test1@test.it", FirstName = "User1", LastName = "User1_1"},
                new User { Id = 2, Email = "test2@test.it", FirstName = "User2", LastName = "User2_1"},
                new User { Id = 3, Email = "test3@test.it", FirstName = "User3", LastName = "User3_1"},
                new User { Id = 4, Email = "test4@test.it", FirstName = "User4", LastName = "User4_1"},
                new User { Id = 5, Email = "test5@test.it", FirstName = "User5", LastName = "User5_1"},
                new User { Id = 6, Email = "test6@test.it", FirstName = "User6", LastName = "User6_1"},
                new User { Id = 7, Email = "test7@test.it", FirstName = "User1", LastName = "User1_1"},
                new User { Id = 8, Email = "test8@test.it", FirstName = "User2", LastName = "User2_1"},
                new User { Id = 9, Email = "test9@test.it", FirstName = "User3", LastName = "User3_1"},
                new User { Id = 10, Email = "test10@test.it", FirstName = "User4", LastName = "User4_1"},
                new User { Id = 11, Email = "test11@test.it", FirstName = "User5", LastName = "User5_1"},
                new User { Id = 12, Email = "test12@test.it", FirstName = "User6", LastName = "User6_1"},
                new User { Id = 13, Email = "test13@test.it", FirstName = "User6", LastName = "User6_1"}
            };

        private UsersController InitializeUserController(int userId = 1)
        {
            var mockUserManager = new Mock<FakeUserManager>();
            var mockRoleManager = new Mock<FakeRoleManager>();

            mockUserManager.Setup(userManager => userManager.Users)
                .Returns(expectedUsers.AsQueryable());
            mockUserManager.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult((userId > 0 && userId <= expectedUsers.Count()) ? expectedUsers[userId - 1] : null));
            IList<string> expectedRolesPerUser = new List<string> { "Administrator", "User" };
            mockUserManager.Setup(userManager => userManager.GetRolesAsync(It.IsAny<User>())).Returns(Task.FromResult(expectedRolesPerUser));

            mockRoleManager.Setup(roleManager => roleManager.Roles)
                .Returns(new List<IdentityRole<int>> { }.AsQueryable());

            var userController = new UsersController(mockUserManager.Object, mockRoleManager.Object);

            return userController;
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void IndexReturnsViewResultWithListOfUsersPerPageTest(int pageNumber)
        {
            // Arrange
            var usersController = InitializeUserController();

            // Act
            var result = usersController.Index(pageNumber);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);

            Assert.Equal((pageNumber * usersController.PageSize <= expectedUsers.Count())? 
                usersController.PageSize 
                : expectedUsers.Count() - (pageNumber - 1) * usersController.PageSize
                , (viewResult?.Model as UsersViewModel).Users.Count);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(1)]
        public void EditReturnsViewResultWithSelectedUser(int userId)
        {
            // Arrange
            var userController = InitializeUserController(userId);

            // Act
            var result = userController.Edit(userId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);

            Assert.Equal(expectedUsers[userId - 1].Id, (viewResult?.Model as UserEditViewModel).User.Id);
        }

        [Theory]
        [InlineData(27)]
        [InlineData(0)]
        public void EditReturnsNotFoundForGetMethod(int userId)
        {
            // Arrange
            var userController = InitializeUserController(userId);

            // Act
            var result = userController.Edit(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Theory]
        [InlineData(9)]
        [InlineData(2)]
        public void EditReturnsRedirectToActionIndex(int userId)
        {
            // Arrange
            var userController = InitializeUserController(userId);

            // Act
            var result = userController.Edit(new UserEditViewModel
            {
                User = new User
                {
                    Id = 1,
                    FirstName = "q",
                    LastName = "w",
                    Email = "test123@test.it"
                },
                ActiveRoles = new List<string>
                {
                    "User"
                },
                AllRoles = new List<IdentityRole<int>>
                {
                    new IdentityRole<int>
                    {
                        Id = 1,
                        Name = "Admin"
                    },
                    new IdentityRole<int>
                    {
                        Id = 2,
                        Name = "User"
                    }
                }
            });

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult?.ActionName);
        }

        [Theory]
        [InlineData(31)]
        [InlineData(-1)]
        public void EditReturnsNotFoundForPostMethod(int userId)
        {
            // Arrange
            var userController = InitializeUserController(userId);

            // Act
            var result = userController.Edit(new UserEditViewModel
            {
                User = new User
                {
                    Id = 1,
                    FirstName = "q",
                    LastName = "w",
                    Email = "test123@test.it"
                },
                ActiveRoles = new List<string>
                {
                    "User"
                },
                AllRoles = new List<IdentityRole<int>>
                {
                    new IdentityRole<int>
                    {
                        Id = 1,
                        Name = "Admin"
                    },
                    new IdentityRole<int>
                    {
                        Id = 2,
                        Name = "User"
                    }
                }
            });

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Theory]
        [InlineData(31)]
        [InlineData(-1)]
        public void DeleteReturnsNotFoundForPostMethod(int userId)
        {
            // Arrange
            var userController = InitializeUserController(userId);

            // Act
            var result = userController.Delete(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(11)]
        public void DeleteReturnsRedirectToActionIndex(int userId)
        {
            // Arrange
            var userController = InitializeUserController(userId);

            // Act
            var result = userController.Delete(userId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult?.ActionName);
        }
    }
}
