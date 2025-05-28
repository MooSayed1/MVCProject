using Company.Data.Models;

namespace Company.Web.Models;

public class DepartmentViewModel
{
    
    public int id { get; set; }
    public int Code;
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreateAt { get; set; }
    public ICollection<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
}