using Company.Service.Interfaces.Employee.Dto;

namespace Company.Service.Interfaces.Employee;
using Company.Data.Models;

public interface IEmployeeService
{
    
    EmployeeDto? GetById(int? id);
    IEnumerable<EmployeeDto> GetAll();
    void Add(EmployeeDto? department);
    void Update(EmployeeDto? department);
    void Delete(EmployeeDto department);
    
    IEnumerable<EmployeeDto>? GetEmployeeByName(string name);
}