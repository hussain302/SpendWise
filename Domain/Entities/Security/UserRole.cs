using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using static Shared.Persistance.Schemas.DatabaseSchemas;

namespace Domain.Entities.Security;

[Table(UserRoleSchema.TableName, Schema = UserRoleSchema.SchemaName)]
public sealed class UserRole : AuditableEntity<Guid>
{
    [ForeignKey(nameof(RoleId))]
    public Guid RoleId { get; set; }

    public ApplicationRole Role { get; set; }


    [ForeignKey(nameof(UserId))]
    public Guid UserId { get; set; }

    public User User { get; set; }
}
