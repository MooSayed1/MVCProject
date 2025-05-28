using Company.Data.Contexts;
using Company.Data.Models;
using Company.Repository.Interfaces;

namespace Company.Repository.Repositories;

public class EmployeeRepo : GenericRepo<Employee>, IEmployeeRepo
{
    private readonly CompanyDbContext _context;
    public EmployeeRepo(CompanyDbContext context) : base(context)
    {
        _context = context;
    }

    public int GetEmployeeCount(int departmentId)
    {
        var empCount = _context.Employees.Count();
        return empCount;
    }

    public IEnumerable<Employee>? GetEmployeeByName(string name)
    {
        return _context.Employees.Where(x => x.PhoneNumber != null && (x.Name.Trim().ToLower().Contains(name.Trim().ToLower())||
                                        x.Email.Trim().ToLower().Contains(name.Trim().ToLower())||
                                        x.PhoneNumber.Trim().ToLower().Contains(name.Trim().ToLower()))&& x.IsDeleted == false);
    }
}