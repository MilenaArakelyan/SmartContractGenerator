using System.ComponentModel.DataAnnotations;

namespace ProjectS5.Core.Users.Models;

public class RegisterUserModel
{
    [Required(ErrorMessage = $"{nameof(Username)} is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = $"{nameof(Email)} is required")]
    [EmailAddress(ErrorMessage = "Email is not a valid email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = $"{nameof(Password)} is required")]
    [MinLength(8, ErrorMessage = "Password should contain at least 8 characters")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirming password is required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; }
}
