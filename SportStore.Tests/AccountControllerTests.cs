using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStore.Models.Entities;
using SportStore.Tests.FakeManagers;
using SportStore.WebUI.Controllers;
using SportStore.WebUI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SportStore.Tests
{
    public class AccountControllerTests
    {
        public AccountController AccountControllerInitializer(IdentityResult createResult, SignInResult signInResult, bool isLocalUrl)
        {

            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    UserName = "Test",
                    Email = "test@test.it"
                }

            }.AsQueryable();

            var fakeUserManager = new Mock<FakeUserManager>();
            var mockUrl = new Mock<IUrlHelper>();

            mockUrl.Setup(url => url.IsLocalUrl(It.IsAny<string>()))
                .Returns(isLocalUrl);

            fakeUserManager.Setup(userManager => userManager.Users)
                .Returns(users);

            fakeUserManager.Setup(userManager => userManager.DeleteAsync(It.IsAny<User>()))
             .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(userManager => userManager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(createResult);
            fakeUserManager.Setup(userManager => userManager.UpdateAsync(It.IsAny<User>()))
          .ReturnsAsync(IdentityResult.Success);

            var fakeSignInManager = new Mock<FakeSignInManager>();

            fakeSignInManager.Setup(
                    signInManager => signInManager.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(),true, false))
                .Returns(Task.FromResult(signInResult));

            var accountController = new AccountController(fakeUserManager.Object, fakeSignInManager.Object);
            accountController.Url = mockUrl.Object;
            return accountController;
        }

        [Fact]
        public void GetRegisterReturnsViewResult()
        {
            // Arrange
            var accountController = AccountControllerInitializer(IdentityResult.Success, SignInResult.Success, true);

            // Act
            var result = accountController.Register();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Null(viewResult?.ViewName);
        }

        [Fact]
        public void RegisterReturnsRedirectToAction()
        {
            // Arrange
            var accountController = AccountControllerInitializer(IdentityResult.Success, SignInResult.Success, true);
            var registerViewModel = new RegisterViewModel
            {
                Email = "qe@test.it",
                Username = "Qe",
            };

            // Act
            var result = accountController.Register(registerViewModel);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

            Assert.Equal("Home", viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void RegisterReturnsRegisterViewModelWithErrorMessages()
        {

            // Arrange
            var accountController = AccountControllerInitializer(IdentityResult.Failed(new IdentityError { Description = "test" }), SignInResult.Success, true);
            var registerViewModel = new RegisterViewModel
            {
                Email = "qe@test.it",
                Username = "Qe",
            };

            // Act
            var result = accountController.Register(registerViewModel);

            // Assert
            Assert.NotEqual(0, accountController.ModelState.ErrorCount);
        }

        [Theory]
        [InlineData("/test1/test2")]
        public void LoginReturnsViewResult(string expectedUrl)
        {
            // Arrange
            var accountController = AccountControllerInitializer(IdentityResult.Success, SignInResult.Success, true);

            // Act
            var result = accountController.Login(expectedUrl);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedUrl, (viewResult.Model as LoginViewModel).ReturnUrl);
        }

        [Theory]
        [InlineData("user1", "password", "~/Home/Index")]
        public void LoginReturnsRedirectToReturnUrl(string login, string password, string url)
        {
            // Arrange
            var accountController = AccountControllerInitializer(IdentityResult.Success, SignInResult.Success, true);

            // Act
            var result = accountController.Login(new LoginViewModel { Login = login, Password = password, RememberMe = true, ReturnUrl = url});

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result.Result);

            Assert.Equal(url, viewResult.Url);
        }

        [Theory]
        [InlineData("user1", "password", "~/Test1/Test2")]
        public void LoginReturnsRedirectToActionWhenReturnUrlIsNotLocal(string login, string password, string url)
        {
            // Arrange
            var accountController = AccountControllerInitializer(IdentityResult.Success, SignInResult.Success, false);

            // Act
            var result = accountController.Login(new LoginViewModel { Login = login, Password = password, RememberMe = true, ReturnUrl = url });

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

            Assert.Equal("Home", viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Theory]
        [InlineData("user1", "password", "~/Test1/Test2")]
        public void LoginReturnsViewResultWidhErrorsInModelState(string login, string password, string url)
        {
            // Arrange
            var accountController = AccountControllerInitializer(IdentityResult.Success, SignInResult.Failed, false);

            // Act
            var result = accountController.Login(new LoginViewModel { Login = login, Password = password, RememberMe = true, ReturnUrl = url });

            // Assert
            Assert.NotEqual(0, accountController.ModelState.ErrorCount);
        }


        [Fact]
        public void LogoutReturnsRedirectToAction()
        {
            // Arrange 
            var accountController = AccountControllerInitializer(IdentityResult.Success, SignInResult.Success, true);

            // Act
            var result = accountController.Logout();

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

            Assert.Equal("Home", viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void AccessDeniedReturnsViewResult()
        {
            // Arrange 
            var accountController = AccountControllerInitializer(IdentityResult.Success, SignInResult.Success, true);

            // Act
            var result = accountController.AccessDenied();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.Null(viewResult.ViewName);
        }
    }
}
