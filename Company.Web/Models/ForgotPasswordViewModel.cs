using System.ComponentModel.DataAnnotations;

namespace Company.Web.Models;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; init; }
}