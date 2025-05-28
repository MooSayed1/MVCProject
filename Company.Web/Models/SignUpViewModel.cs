using System.ComponentModel.DataAnnotations;

namespace Company.Web.Models;

public class SignUpViewModel
{
    [Required(ErrorMessage = "FirstName is required")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$", 
    ErrorMessage = "Password must be at least 6 characters long, and must include at least one uppercase letter, one lowercase letter, and one number.")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password do not match.")]
    public string ConfirmPassword { get; set; }
    public bool IsAgree { get; set; }
}