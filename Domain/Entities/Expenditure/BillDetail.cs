#region Using Directives
using Domain.Contracts;
using Domain.Entities.Security;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Shared.Persistance.Schemas.DatabaseSchemas;
#endregion

namespace Domain.Entities.Expenditure;

[Table(BillDetailSchema.TableName, Schema = BillDetailSchema.SchemaName)]
public class BillDetail : AuditableEntity<Guid>
{
    public Guid BillId { get; set; }
    public Bill Bill { get; set; }

    [ForeignKey(nameof(SharedWith))]
    public Guid SharedWithId { get; set; }
    public ApplicationUser SharedWith { get; set; }

    [Required]
    public decimal ShareAmount { get; set; }

    [Required]
    public bool PaidOnSpot { get; set; }
}
