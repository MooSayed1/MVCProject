using System.Reflection;
using Company.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Company.Data.Contexts;

public class CompanyDbContext : IdentityDbContext<ApplicationUser> 
{
    public CompanyDbContext()
    {
        
    }
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseSqlServer("server=localhost;database=Company;user=sa;password=Password123");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<BaseEntity>().HasQueryFilter(x => !x.IsDeleted);  // Global filter for soft delete
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
}