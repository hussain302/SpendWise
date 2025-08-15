using Domain.Entities.Ownership;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Security;

public class ApplicationUser : IdentityUser<Guid>
{
    public bool IsActive { get; set; }

    public bool IsSoloUser { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public Guid? OrganizationId { get; set; }
    public Organization Organization { get; set; }
}
