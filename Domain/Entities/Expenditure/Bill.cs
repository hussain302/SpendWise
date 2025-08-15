#region Using Directives
using Domain.Contracts;
using System.ComponentModel.DataAnnotations;
using static Shared.Persistance.Schemas.DatabaseSchemas;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Ownership;
using Domain.Entities.Security;
#endregion

namespace Domain.Entities.Expenditure;

[Table(BillSchema.TableName, Schema = BillSchema.SchemaName)]

public sealed class Bill : AuditableEntity<Guid>
{
    [Required]
    [StringLength(50)]
    public string Title { get; set; }

    [Required]
    public decimal BillAmount { get; set; }

    [Required]
    [StringLength(int.MaxValue)]
    public string Description { get; set; }

    [Required]
    public DateTime BilledOn { get; set; }

    [Required]
    public bool IsEmailSent { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; }

    [ForeignKey(nameof(PaidBy))]
    public Guid PaidById { get; set; }
    public ApplicationUser PaidBy { get; set; }

    public ICollection<BillDetail> SharedWithUsers { get; set; } = [];

}
