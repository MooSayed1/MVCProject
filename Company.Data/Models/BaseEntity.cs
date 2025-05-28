namespace Company.Data.Models;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.Now;
    public DateTime? DeleteAt { get; set; } 
    public bool IsDeleted { get; set; }
    
}