using Company.Service.Interfaces.Employee.Dto;

namespace Company.Service.Interfaces.Dto;

public class DepartmentDto
{
    public int Id { get; set; }
    public int? Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreateAt { get; set; }
    public ICollection<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
}