namespace Company.Data.Models;

public class Department : BaseEntity
{
    public int Code { get; set; }
    public string Name { get; set; }
    public DateTime CreateAt { get; set; }
    public string? Description { get; set; }
    public ICollection<Employee> Employees { get; set; }
}