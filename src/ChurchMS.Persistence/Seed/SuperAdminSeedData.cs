using ChurchMS.Domain.Entities;
using ChurchMS.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChurchMS.Persistence.Seed;

/// <summary>
/// Seeds the initial SuperAdmin user on application startup.
/// </summary>
public static class SuperAdminSeedData
{
    public static async Task SeedSuperAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        var superAdminEmail = "admin@churchms.com";
        var existingUser = await userManager.FindByEmailAsync(superAdminEmail);

        if (existingUser is not null)
            return;

        var superAdmin = new AppUser
        {
            UserName = superAdminEmail,
            Email = superAdminEmail,
            FirstName = "System",
            LastName = "Administrator",
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(superAdmin, "Password@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(superAdmin, AppConstants.Roles.SuperAdmin);
            logger.LogInformation("SuperAdmin user seeded successfully.");
        }
        else
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError("Failed to seed SuperAdmin: {Errors}", errors);
        }
    }
}
