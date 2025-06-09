using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NetProject.Controllers;
using NetProject.ViewModels;
using Xunit;
using IdentitySignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace NetProject.Tests.Controllers
{
    public class AccountControllerTests
    {
        private Mock<UserManager<IdentityUser>>    _userManagerMock;
        private Mock<SignInManager<IdentityUser>>  _signInManagerMock;
        private AccountController                   _controller;

        public AccountControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactoryMock   = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object,
                contextAccessorMock.Object,
                claimsFactoryMock.Object,
                null, null, null, null);

            _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object);
        }

        [Fact]
        public void Register_Get_ReturnsView()
        {
            var result = _controller.Register();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Register_Post_InvalidModel_ReturnsViewWithModel()
        {
            _controller.ModelState.AddModelError("Email", "Required");
            var vm = new RegisterViewModel();

            var result = await _controller.Register(vm) as ViewResult;

            Assert.NotNull(result);
            Assert.Same(vm, result.Model);
        }

        [Fact]
        public async Task Register_Post_SuccessfulCreation_RedirectsToDashboard()
        {
            var vm = new RegisterViewModel { Email = "a@b.com", Password = "Pass123!" };
            _userManagerMock
                .Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), vm.Password))
                .ReturnsAsync(IdentityResult.Success);
            _signInManagerMock
                .Setup(s => s.SignInAsync(It.IsAny<IdentityUser>(), false, null))
                .Returns(Task.CompletedTask);

            var result = await _controller.Register(vm) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Dashboard", result.ActionName);
            Assert.Equal("Account",   result.ControllerName);
        }

        [Fact]
        public async Task Register_Post_FailedCreation_AddsModelErrorAndReturnsView()
        {
            var vm = new RegisterViewModel { Email = "a@b.com", Password = "Pass123!" };
            var identityErrors = new[] { new IdentityError { Description = "Bad" } };
            _userManagerMock
                .Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), vm.Password))
                .ReturnsAsync(IdentityResult.Failed(identityErrors));

            var result = await _controller.Register(vm) as ViewResult;

            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Contains("Bad", _controller.ModelState[string.Empty].Errors[0].ErrorMessage);
            Assert.Same(vm, result.Model);
        }

        [Fact]
        public void Login_Get_ReturnsView()
        {
            var result = _controller.Login();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Login_Post_InvalidModel_ReturnsView()
        {
            _controller.ModelState.AddModelError("Email", "Required");
            var vm = new LoginViewModel();

            var result = await _controller.Login(vm) as ViewResult;

            Assert.NotNull(result);
            Assert.Same(vm, result.Model);
        }

        [Fact]
        public async Task Login_Post_SuccessfulSignIn_RedirectsToDashboard()
        {
            var vm = new LoginViewModel { Email = "a@b.com", Password = "Pass123!" };
            _signInManagerMock
                .Setup(s => s.PasswordSignInAsync(vm.Email, vm.Password, false, false))
                .ReturnsAsync(IdentitySignInResult.Success);

            var result = await _controller.Login(vm) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Dashboard", result.ActionName);
            Assert.Equal("Account",   result.ControllerName);
        }

        [Fact]
        public async Task Login_Post_FailedSignIn_AddsModelErrorAndReturnsView()
        {
            var vm = new LoginViewModel { Email = "a@b.com", Password = "Pass123!" };
            _signInManagerMock
                .Setup(s => s.PasswordSignInAsync(vm.Email, vm.Password, false, false))
                .ReturnsAsync(IdentitySignInResult.Failed);

            var result = await _controller.Login(vm) as ViewResult;

            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Contains("Invalid login attempt.", _controller.ModelState[string.Empty].Errors[0].ErrorMessage);
            Assert.Same(vm, result.Model);
        }
    }
}
