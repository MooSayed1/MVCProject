using Company.Data.Models;

namespace Company.Repository.Interfaces;

public interface IGenericRepo<T> where T : BaseEntity
{
    public T? GetById(int id);
    public IEnumerable<T> GetAll();
    public void Add(T entity);
    public void Update(T entity);
    public void Delete(T entity);
    
}