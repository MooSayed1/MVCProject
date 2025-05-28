using System.ComponentModel.DataAnnotations;
using Company.Service.Interfaces.Dto;
using Microsoft.AspNetCore.Http;

namespace Company.Service.Interfaces.Employee.Dto;

public class EmployeeDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Age is required.")]
    [Range(18, 100, ErrorMessage = "Age must be between 18 and 100.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Salary is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
    public decimal Salary { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? HiringDate { get; set; }

    public IFormFile? Image { get; set; }

    public string? ImageUrl { get; set; }

    public DepartmentDto? Department { get; set; }

    public int? DepartmentId { get; set; }
}
