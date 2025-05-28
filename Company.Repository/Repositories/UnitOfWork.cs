using Company.Data.Contexts;
using Company.Repository.Interfaces;

namespace Company.Repository.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CompanyDbContext _context;
    public UnitOfWork(CompanyDbContext context)
    {
        _context = context;
        DepartmentRepo = new DepartmentRepo(_context);
        EmployeeRepo = new EmployeeRepo(_context);
    }
    
    public IDepartmentRepo DepartmentRepo { get; set; }
    public IEmployeeRepo EmployeeRepo { get; set; }
    
    public int Complete()
    {
        return _context.SaveChanges();
    }
}