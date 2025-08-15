using Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shared.Security;

namespace Infrastructure.Implementations.Persistence.Contexts.Seeding;
public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, string encrptionKey)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                var applicationRole = new ApplicationRole
                {
                    Name = role,
                    Description = $"{role} role for SpendWise" 
                };
                await roleManager.CreateAsync(applicationRole);
            }
        }

        var adminUser = new ApplicationUser
        {
            UserName = "hussain.admin",
            Email = "hussain.admin@spendwise.com",
            EmailConfirmed = true,
            IsActive = true,
            PhoneNumber = "+923084299454"
        };

        if (await userManager.FindByEmailAsync(adminUser.Email) == null)
        {
            string password = "Admin@123";
            var result = await userManager.CreateAsync(adminUser, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

}