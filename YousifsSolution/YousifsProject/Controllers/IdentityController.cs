using Microsoft.AspNetCore.Mvc;
using YousifsProject.Services.Implementations;
using YousifsProject.Views.Identity;

namespace YousifsProject.Controllers
{
    [Route("account")]
    public class IdentityController : Controller
    {
        readonly IdentityServiceDB service;

        public IdentityController(IdentityServiceDB service)
        {
            this.service = service;
        }

        [HttpGet("login")]
        public ActionResult Login() => View();

        [HttpGet("Signup")]
        public ActionResult Signup(int id) => View();

        [HttpPost("Signup")]
        public async Task<ActionResult> SignupAsync(SignupVM model)
        {
            if (!ModelState.IsValid)
                return View();

            // Try to register user
            var errorMessage = await service.TryRegisterUserAsync(model);
            if (errorMessage != null)
            {
                if (errorMessage.Contains("Password", StringComparison.OrdinalIgnoreCase))
                    ModelState.AddModelError("Password", errorMessage);
                else
                    ModelState.AddModelError("Username", errorMessage);
                return View();
            }

            return RedirectToAction(nameof(HousesController.Index), "Houses");
        }

        [HttpGet("Signout")]
        public async Task<ActionResult> Signout()
        {
            service.SignOutUserAsync();
            return RedirectToAction(nameof(Login));
        }

    }
}
