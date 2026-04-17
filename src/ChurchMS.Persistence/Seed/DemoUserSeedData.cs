using ChurchMS.Domain.Entities;
using ChurchMS.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChurchMS.Persistence.Seed;

/// <summary>
/// Seeds demo users (one per role) for the parent church. Password for all: Password@123
/// </summary>
public static class DemoUserSeedData
{
    private static readonly Guid ParentChurchId = Guid.Parse("A0000001-0000-0000-0000-000000000001");
    private static readonly Guid ChildChurch1Id = Guid.Parse("A0000001-0000-0000-0000-000000000002");

    private static readonly (Guid Id, string Email, string FirstName, string LastName, string Role, Guid ChurchId)[] Users =
    [
        (Guid.Parse("B0000001-0000-0000-0000-000000000001"), "central@eeg-grace.org",     "Éric",      "Nguéma",     AppConstants.Roles.CentralAdmin,       ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000002"), "admin.church@eeg-grace.org", "Hélène",    "Tchouameni", AppConstants.Roles.ChurchAdmin,         ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000003"), "it@eeg-grace.org",           "Célestin",  "Mbappé",     AppConstants.Roles.ITManager,           ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000004"), "secretariat@eeg-grace.org",  "Béatrice",  "Fotso",      AppConstants.Roles.Secretary,           ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000005"), "tresorier@eeg-grace.org",    "André",     "Kamga",      AppConstants.Roles.Treasurer,           ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000006"), "dept.head@eeg-grace.org",    "François",  "Nganou",     AppConstants.Roles.DepartmentHead,      ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000007"), "dept.tres@eeg-grace.org",    "Géraldine", "Tagne",      AppConstants.Roles.DepartmentTreasurer, ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000008"), "enseignant@eeg-grace.org",   "Théophile", "Nkoulou",    AppConstants.Roles.Teacher,             ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000009"), "evangelisme@eeg-grace.org",  "Moïse",     "Essomba",    AppConstants.Roles.EvangelismLeader,    ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000000A"), "multimedia@eeg-grace.org",   "Noël",      "Atangana",   AppConstants.Roles.MultimediaManager,   ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000000B"), "logistique@eeg-grace.org",   "Raphaël",   "Djomgang",   AppConstants.Roles.LogisticsManager,    ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000000C"), "membre1@eeg-grace.org",      "Joséphine", "Mbouda",     AppConstants.Roles.Member,              ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000000D"), "membre2@eeg-grace.org",      "Émmanuel",  "Tchinda",    AppConstants.Roles.Member,              ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000000E"), "membre3@eeg-grace.org",      "Élisabeth", "Nkwenti",    AppConstants.Roles.Member,              ChildChurch1Id),
    ];

    public static async Task SeedDemoUsersAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        foreach (var (id, email, firstName, lastName, role, churchId) in Users)
        {
            var existing = await userManager.FindByEmailAsync(email);
            if (existing is not null)
                continue;

            var user = new AppUser
            {
                Id = id,
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                ChurchId = churchId,
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, "Password@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
                logger.LogInformation("Demo user {Email} ({Role}) seeded.", email, role);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                logger.LogError("Failed to seed {Email}: {Errors}", email, errors);
            }
        }
    }
}
