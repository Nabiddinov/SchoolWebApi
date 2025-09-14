namespace SchoolWebApi.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? LastUpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}