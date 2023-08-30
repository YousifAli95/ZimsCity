using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using ZimsCityProject.Controllers;
using ZimsCityProject.Services.Interfaces;
using ZimsCityProject.Views.Identity.ViewModels;

namespace Tests
{
    public class IdentityControllerTests
    {
        [Fact]
        public async Task Signup_ValidModel_RedirectsToIndex()
        {
            var mockIdentityService = new Mock<IIdentityService>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<ClaimsIdentity>();

            // Simulate an authenticated user by setting IsAuthenticated to true
            mockIdentity.Setup(identity => identity.IsAuthenticated).Returns(false);
            mockHttpContext.Setup(context => context.User.Identity).Returns(mockIdentity.Object);

            var controller = new IdentityController(mockIdentityService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var model = new SignupVM();

            // Act
            var result = await controller.SignupAsync(model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(HousesController.Index), result.ActionName);
        }

        [Fact]
        public async Task Signup_InvalidModel_ReturnsViewWithModelError()
        {
            var mockIdentityService = new Mock<IIdentityService>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<ClaimsIdentity>();

            mockIdentity.Setup(identity => identity.IsAuthenticated).Returns(false);
            mockHttpContext.Setup(context => context.User.Identity).Returns(mockIdentity.Object);

            var controller = new IdentityController(mockIdentityService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            // Simulate invalid model state by adding a model error
            controller.ModelState.AddModelError("Password", "Invalid password");

            var model = new SignupVM();

            // Act
            var result = await controller.SignupAsync(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Login_AlreadySignedIn_RedirectsToIndex()
        {
            var mockIdentityService = new Mock<IIdentityService>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<ClaimsIdentity>();

            mockIdentity.Setup(identity => identity.IsAuthenticated).Returns(true);
            mockHttpContext.Setup(context => context.User.Identity).Returns(mockIdentity.Object);

            var controller = new IdentityController(mockIdentityService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var model = new LoginVM();

            // Act
            var result = await controller.LoginAsync(model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(HousesController.Index), result.ActionName);
        }

        [Fact]
        public async Task Login_ValidModel_RedirectsToIndex()
        {
            var mockIdentityService = new Mock<IIdentityService>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<ClaimsIdentity>();

            mockIdentity.Setup(identity => identity.IsAuthenticated).Returns(false);
            mockHttpContext.Setup(context => context.User.Identity).Returns(mockIdentity.Object);
            // Configure the mockIdentityService to return true when TryLoginUserAsync is called
            mockIdentityService.Setup(service => service.TryLoginUserAsync(It.IsAny<LoginVM>())).ReturnsAsync(true);

            var controller = new IdentityController(mockIdentityService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var model = new LoginVM();

            // Act
            var result = await controller.LoginAsync(model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(HousesController.Index), result.ActionName);
        }

        [Fact]
        public async Task Login_InvalidModel_RedirectsToIndex()
        {
            var mockIdentityService = new Mock<IIdentityService>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<ClaimsIdentity>();

            mockIdentity.Setup(identity => identity.IsAuthenticated).Returns(false);
            mockHttpContext.Setup(context => context.User.Identity).Returns(mockIdentity.Object);

            // Configure the mockIdentityService to return true when TryLoginUserAsync is called
            mockIdentityService.Setup(service => service.TryLoginUserAsync(It.IsAny<LoginVM>())).ReturnsAsync(true);

            var controller = new IdentityController(mockIdentityService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            controller.ModelState.AddModelError("Password", "Invalid password");

            var model = new LoginVM();

            // Act
            var result = await controller.LoginAsync(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async Task Login_LoginFailed_RedirectsToLogin()
        {
            var mockIdentityService = new Mock<IIdentityService>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<ClaimsIdentity>();

            mockIdentity.Setup(identity => identity.IsAuthenticated).Returns(false);
            mockHttpContext.Setup(context => context.User.Identity).Returns(mockIdentity.Object);
            // Configure the mockIdentityService to return true when TryLoginUserAsync is called
            mockIdentityService.Setup(service => service.TryLoginUserAsync(It.IsAny<LoginVM>())).ReturnsAsync(false);

            var controller = new IdentityController(mockIdentityService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var model = new LoginVM();

            // Act
            var result = await controller.LoginAsync(model) as ViewResult; ;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Signout_WithAuthorizedUser_RedirectsToLogin()
        {
            // Arrange
            var mockIdentityService = new Mock<IIdentityService>();
            var controller = new IdentityController(mockIdentityService.Object);
            var expectedRedirectAction = nameof(IdentityController.Login);

            // Act
            var result = await controller.Signout() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRedirectAction, result.ActionName);
        }
    }
}
