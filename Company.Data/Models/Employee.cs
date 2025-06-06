using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Data.Models;

public class Employee : BaseEntity
{
    public string Name { get; set; }
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? HiringDate { get; set; }
    public string? ImageUrl { get; set; }
    
    [ForeignKey(nameof(DepartmentId))]
    public Department? Department { get; set; }
    public int? DepartmentId { get; set; }
}