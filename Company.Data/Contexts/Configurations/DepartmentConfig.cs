using Company.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Data.Contexts.Configurations;

public class DepartmentConfig : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Department");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).HasMaxLength(50).IsRequired();
        builder.Property(d => d.Description).HasMaxLength(200);
        
    }
}