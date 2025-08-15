namespace Domain.Contracts;
public abstract class AuditableEntity<TId> : Entity<TId>
{
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; } = string.Empty;
    public DateTime? LastModifiedOn { get; set; }

    public void Created(string createdBy, DateTime createdAtUtc)
    {
        CreatedOn = createdAtUtc;
        CreatedBy = createdBy;
    }

    public void Updated(string updatedBy, DateTime updatedAtUtc)
    {
        LastModifiedOn = updatedAtUtc;
        LastModifiedBy = updatedBy;
    }
}