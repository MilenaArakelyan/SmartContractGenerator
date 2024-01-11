using Blazored.LocalStorage;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using ProjectS5.Core.Services.Email;
using ProjectS5.Core.Users.Models;
using System.Security.Claims;

namespace ProjectS5.Core.Auth
{
    public class CustomStateProvider(IAuthManager authManager, IEmailService emailService, IMemoryCache cache) : AuthenticationStateProvider
    {
        private static CurrentUser _currentUser;
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (_currentUser?.IsAuthenticated is true)
            {
                identity = new ClaimsIdentity(_currentUser.Claims.Select(s => new Claim(s.Key, s.Value)), "custom");
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task Login(LoginRequestModel loginModel)
        {
            await authManager.Login(loginModel);
        }
        public async Task Register(RegisterUserModel registerModel)
        {
            await authManager.Register(registerModel);
        }

        public async Task SendOtpAsync(string email)
        {
            string template = @"<!DOCTYPE html>
<html>
<head>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 20px;
        }
        .email-container {
            background-color: #ffffff;
            max-width: 600px;
            margin: auto;
            padding: 20px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .header {
            background-color: #4CAF50;
            color: white;
            padding: 10px;
            text-align: center;
        }
        .content {
            margin-top: 20px;
        }
        .footer {
            margin-top: 20px;
            text-align: center;
            font-size: 0.8em;
            color: #888;
        }
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""header"">
            Confirmation
        </div>
        <div class=""content"">
            <p>Hello,</p>
            <p>Your One-Time Password (OTP) for accessing your account is:</p>
            <h2 style=""text-align:center;"">{{OTP}}</h2>
            <p>Please enter this code to proceed. The code is valid for 2 minutes.</p>
        </div>
        <div class=""footer"">
            <p>Thank you for using Smart Contracts with us!</p>
        </div>
    </div>
</body>
</html>
";
            int sixDigitNumber = new Random().Next(100000, 1000000);
            template = template.Replace("{{OTP}}", sixDigitNumber.ToString());
            await emailService.SendEmailAsync(email, "OTP to login", template, isHtmlContent: true);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            cache.Set(email, sixDigitNumber, cacheEntryOptions);
        }

        public bool VerifyUserOtp(string email, int otp)
        {
            var r = cache.TryGetValue(email, out int cachedOtp) && cachedOtp == otp;
            if (r)
            {
                cache.Remove(email);
                SignIn(email);
            }
            return r;
        }

        public void SignIn(string email)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, email)
            };

            var identity = new ClaimsIdentity(claims, "custom");
            var user = new ClaimsPrincipal(identity);

            _currentUser = new CurrentUser
            {
                IsAuthenticated = true,
                Claims = user.Claims.ToDictionary(c => c.Type, c => c.Value)
            };
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task SignOutAsync()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }
    }
}
