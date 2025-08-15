using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Security;

public sealed class ApplicationRole : IdentityRole<Guid>
{  
    [Required]
    public string Description { get; set; }
}
