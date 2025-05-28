using Company.Data.Contexts;
using Company.Data.Models;
using Company.Repository.Interfaces;

namespace Company.Repository.Repositories;

public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
{
    private readonly CompanyDbContext _context;
    
    public GenericRepo(CompanyDbContext context)
    {
        _context = context;
    }
    
    public T? GetById(int id)
    {
        var entity = _context.Set<T>().Find(id);
        return entity ?? null;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().Where(d => d.IsDeleted == false).ToList();
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        // _context.Set<T>().Remove(entity); 
        entity.IsDeleted = true;
    }
}