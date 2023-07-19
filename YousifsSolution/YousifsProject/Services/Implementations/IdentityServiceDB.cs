﻿using Microsoft.AspNetCore.Identity;
using YousifsProject.Models.Entities;
using YousifsProject.Services.Interfaces;
using YousifsProject.Views.Identity;

namespace YousifsProject.Services.Implementations
{

    public class IdentityServiceDB : IIdentityService
    {
        readonly UserManager<IdentityUser> userManager;
        readonly SignInManager<IdentityUser> signInManager;
        public IdentityServiceDB(CityContext cityContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<string?> TryRegisterUserAsync(SignupVM model)
        {
            var user = new IdentityUser
            {
                UserName = model.Username,
            };

            IdentityResult createResult = await userManager.CreateAsync(user, model.Password);

            if (createResult.Succeeded)
            {
                await signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);
            }
            else
            {
                Console.WriteLine(createResult.Errors.First().Description);
            }

            return createResult.Succeeded ? null : createResult.Errors.First().Description;
        }

        public async Task SignOutUserAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<bool> TryLoginUserAsync(LoginVM model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);

            return result.Succeeded;
        }

    }
}