namespace ProjectS5.Core.Users.Models;

public class CurrentUser
{
    public bool IsAuthenticated { get; set; }
    public Dictionary<string, string> Claims { get; set; }
}
