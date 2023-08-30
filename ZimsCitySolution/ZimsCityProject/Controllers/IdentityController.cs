using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZimsCityProject.Services.Interfaces;
using ZimsCityProject.Views.Identity.ViewModels;

namespace ZimsCityProject.Controllers
{
    [Route("account")]
    public class IdentityController : Controller
    {
        readonly IIdentityService _identityService;

        public IdentityController(IIdentityService service)
        {
            this._identityService = service;
        }

        [HttpGet("login")]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToIndex();

            return View();
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginVM model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToIndex();

            if (!ModelState.IsValid)
                return View();

            var loginSucceded = await _identityService.TryLoginUserAsync(model);

            if (!loginSucceded)
            {
                ModelState.AddModelError(nameof(LoginVM.Username), "Invalid username or password");
                return View();
            }

            return RedirectToIndex();

        }

        [HttpGet("Signup")]
        public ActionResult Signup()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToIndex();

            return View();
        }

        [HttpPost("Signup")]
        public async Task<ActionResult> SignupAsync(SignupVM model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToIndex();

            if (!ModelState.IsValid)
                return View();

            var errorMessage = await _identityService.TryRegisterUserAsync(model);
            if (errorMessage != null)
            {
                if (errorMessage.Contains("Password", StringComparison.OrdinalIgnoreCase))
                    ModelState.AddModelError("Password", errorMessage);
                else
                    ModelState.AddModelError("Username", errorMessage);
                return View();
            }

            return RedirectToIndex();
        }

        [Authorize]
        [HttpGet("Signout")]
        public async Task<ActionResult> Signout()
        {
            await _identityService.SignOutUserAsync();
            return RedirectToAction(nameof(Login), nameof(IdentityController).Replace("Controller", ""));
        }

        ActionResult RedirectToIndex() => RedirectToAction(nameof(HousesController.Index), nameof(HousesController).Replace("Controller", ""));
    }

}
