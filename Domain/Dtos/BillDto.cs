using Domain.Contracts;

namespace Domain.Dtos;

public class BillDto : AuditableEntity<Guid>
{
    //public Guid BillId { get; set; }
    public string Title { get; set; }
    public decimal BillAmount { get; set; }
    public string Description { get; set; }
    public DateTime BilledOn { get; set; }
    public bool IsEmailSent { get; set; }
    public string OrganizationName { get; set; }
    public string? PaidByUser { get; set; }
    public List<BillDetailDto> SharedWithUsers { get; set; } = [];
}


public class BillDetailDto
{
    public Guid BillId { get; set; }
    public decimal ShareAmount { get; set; }
    public bool PaidOnSpot { get; set; }
    public string? SharedWithUser { get; set; }
    public Guid? SharedWithUserId { get; set; }
}