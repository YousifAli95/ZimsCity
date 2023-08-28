using ZimsCityProject.Views.Identity.ViewModels;

namespace ZimsCityProject.Services.Interfaces
{
    public interface IIdentityService
    {
        public Task<string?> TryRegisterUserAsync(SignupVM model);

        public Task SignOutUserAsync();

        public Task<bool> TryLoginUserAsync(LoginVM model);


    }
}
