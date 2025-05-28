using Company.Data.Contexts;
using Company.Data.Models;
using Company.Repository.Interfaces;

namespace Company.Repository.Repositories;

public class DepartmentRepo : GenericRepo<Department>, IDepartmentRepo
{
    private readonly CompanyDbContext _context;
    public DepartmentRepo(CompanyDbContext context) : base(context) // Dependency Injection constructor 
    {
        _context = context;
    }

    public int GetDepartmentEmployeeCount(int departmentId)
    {
        var depCount = _context.Departments.Count();
        return depCount;
    }
}