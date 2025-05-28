using Company.Data.Models;

namespace Company.Repository.Interfaces;

public interface IDepartmentRepo : IGenericRepo<Department>
{
    public int GetDepartmentEmployeeCount(int departmentId);
}