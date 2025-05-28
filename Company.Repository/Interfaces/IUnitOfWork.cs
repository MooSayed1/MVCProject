namespace Company.Repository.Interfaces;

public interface IUnitOfWork
{
    public IDepartmentRepo DepartmentRepo { get; set; }
    public IEmployeeRepo EmployeeRepo { get; set; }
    
    int Complete();
}