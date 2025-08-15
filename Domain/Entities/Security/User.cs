using Domain.Contracts;
using Domain.ValueTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Shared.Persistance.Schemas.DatabaseSchemas;

namespace Domain.Entities.Security;

[Table(UserSchema.TableName, Schema = UserSchema.SchemaName)]
public sealed class User : AuditableEntity<Guid>
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    public string? Gender { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    public Address? Address { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }

}
