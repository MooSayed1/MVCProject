using Company.Data.Models;

namespace Company.Repository.Interfaces;

public interface IEmployeeRepo : IGenericRepo<Employee>
{
    public int GetEmployeeCount(int departmentId);
    IEnumerable<Employee>? GetEmployeeByName(string name); // Search by name
}