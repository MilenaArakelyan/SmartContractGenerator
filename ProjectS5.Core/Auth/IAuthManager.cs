using ProjectS5.Core.Users.Models;

namespace ProjectS5.Core.Auth;

public interface IAuthManager
{
    Task Login(LoginRequestModel loginRequest);
    Task Register(RegisterUserModel registerRequest);
    Task Logout();
}
