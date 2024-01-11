using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProjectS5.Core.Users.Models;
using ProjectS5.Data;
using SendGrid.Helpers.Errors.Model;

namespace ProjectS5.Core.Auth;

internal class AuthManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor) : IAuthManager
{
    public async Task Login(LoginRequestModel loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email) ?? throw new BadRequestException("User does not exist");
        var singInResult = await signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
        if (!singInResult.Succeeded)
            throw new BadRequestException("Invalid password");
    }

    public async Task Logout()
    {
        await signInManager.SignOutAsync();
    }

    public async Task Register(RegisterUserModel registerRequest)
    {
        if (await userManager.FindByEmailAsync(registerRequest.Email) is not null)
            throw new BadRequestException("Email is already registered");
        var user = new ApplicationUser
        {
            UserName = registerRequest.Username,
            Email = registerRequest.Email
        };
        var result = await userManager.CreateAsync(user, registerRequest.Password);
        if (!result.Succeeded)
            throw new BadRequestException(string.Join(", ", result.Errors.Select(m => m.Description)));
        await Login(new LoginRequestModel
        {
            Email = registerRequest.Email,
            Password = registerRequest.Password
        });
    }
}
