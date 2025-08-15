using Domain.Contracts;
using System.ComponentModel.DataAnnotations;
using static Shared.Persistance.Schemas.DatabaseSchemas;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Ownership;

[Table(OrganizationSchema.TableName, Schema = OrganizationSchema.SchemaName)]
public class Organization : AuditableEntity<Guid>
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [StringLength(50)]
    public string OwnerName { get; set; }


    [StringLength(int.MaxValue)]
    public string? Details { get; set; }
    
    public int? HeadCount { get; set; }
}
