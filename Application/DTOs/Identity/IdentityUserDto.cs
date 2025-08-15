namespace Application.DTOs.Identity;
public sealed record IdentityUserDto
{
    public Guid UserId { get; init; }
    public string UserName { get; init; }
    public string Email { get; init; }
    public bool IsActive { get; init; }
    public List<string> Roles { get; init; }
}