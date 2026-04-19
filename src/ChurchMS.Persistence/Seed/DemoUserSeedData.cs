using ChurchMS.Domain.Entities;
using ChurchMS.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChurchMS.Persistence.Seed;

/// <summary>
/// Seeds démonstration users (multiple per role, RDC theme).
/// Password for all: Password@123
/// GUIDs match scripts/seed-data.sql so idempotency is preserved across both seeding paths.
/// </summary>
public static class DemoUserSeedData
{
    private static readonly Guid ParentChurchId = Guid.Parse("A0000001-0000-0000-0000-000000000001");
    private static readonly Guid ChildChurch1Id = Guid.Parse("A0000001-0000-0000-0000-000000000002");
    private static readonly Guid ChildChurch2Id = Guid.Parse("A0000001-0000-0000-0000-000000000003");

    private static readonly (Guid Id, string Email, string FirstName, string LastName, string Role, Guid ChurchId)[] Users =
    [
        (Guid.Parse("B0000001-0000-0000-0000-000000000001"), "central1@eeg-grace.cd",     "Patrice",     "Tshilumba",    AppConstants.Roles.CentralAdmin,       ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000002"), "central2@eeg-grace.cd",     "Bénédicte",   "Kabongo",      AppConstants.Roles.CentralAdmin,       ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000003"), "admin.kin@eeg-grace.cd",    "Félicité",    "Mwamba",       AppConstants.Roles.ChurchAdmin,        ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000004"), "admin.lshi@eeg-grace.cd",   "André",       "Ilunga",       AppConstants.Roles.ChurchAdmin,        ChildChurch1Id),
        (Guid.Parse("B0000001-0000-0000-0000-000000000005"), "it1@eeg-grace.cd",          "Célestin",    "Mukendi",      AppConstants.Roles.ITManager,          ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000006"), "it2@eeg-grace.cd",          "Jérémie",     "Kasongo",      AppConstants.Roles.ITManager,          ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000007"), "secretariat1@eeg-grace.cd", "Béatrice",    "Tshibanda",    AppConstants.Roles.Secretary,          ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000008"), "secretariat2@eeg-grace.cd", "Thérèse",     "Kalonji",      AppConstants.Roles.Secretary,          ChildChurch1Id),
        (Guid.Parse("B0000001-0000-0000-0000-000000000009"), "tresorier1@eeg-grace.cd",   "Étienne",     "Mbuyi",        AppConstants.Roles.Treasurer,          ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000000A"), "tresorier2@eeg-grace.cd",   "Clémentine",  "Ngoy",         AppConstants.Roles.Treasurer,          ChildChurch2Id),
        (Guid.Parse("B0000001-0000-0000-0000-00000000000B"), "dept.head1@eeg-grace.cd",   "François",    "Kabila",       AppConstants.Roles.DepartmentHead,     ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000000C"), "dept.head2@eeg-grace.cd",   "Gérard",      "Mulumba",      AppConstants.Roles.DepartmentHead,     ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000000D"), "dept.head3@eeg-grace.cd",   "Marie-Josée", "Lumbala",      AppConstants.Roles.DepartmentHead,     ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000000E"), "dept.tres1@eeg-grace.cd",   "Géraldine",   "Tshiamala",    AppConstants.Roles.DepartmentTreasurer, ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000000F"), "dept.tres2@eeg-grace.cd",   "Cécile",      "Kayembe",      AppConstants.Roles.DepartmentTreasurer, ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000010"), "enseignant1@eeg-grace.cd",  "Théophile",   "Banza",        AppConstants.Roles.Teacher,            ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000011"), "enseignant2@eeg-grace.cd",  "Émilie",      "Mukoko",       AppConstants.Roles.Teacher,            ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000012"), "enseignant3@eeg-grace.cd",  "Noël",        "Kasumba",      AppConstants.Roles.Teacher,            ChildChurch1Id),
        (Guid.Parse("B0000001-0000-0000-0000-000000000013"), "evangelisme1@eeg-grace.cd", "Moïse",       "Tshisekedi",   AppConstants.Roles.EvangelismLeader,   ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000014"), "evangelisme2@eeg-grace.cd", "Daniel",      "Kankonde",     AppConstants.Roles.EvangelismLeader,   ChildChurch2Id),
        (Guid.Parse("B0000001-0000-0000-0000-000000000015"), "multimedia1@eeg-grace.cd",  "Raphaël",     "Bemba",        AppConstants.Roles.MultimediaManager,  ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000016"), "multimedia2@eeg-grace.cd",  "Stéphane",    "Lukusa",       AppConstants.Roles.MultimediaManager,  ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000017"), "logistique1@eeg-grace.cd",  "Éric",        "Lumumba",      AppConstants.Roles.LogisticsManager,   ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-000000000018"), "logistique2@eeg-grace.cd",  "Joseph",      "Kasa-Vubu",    AppConstants.Roles.LogisticsManager,   ChildChurch1Id),
        (Guid.Parse("B0000001-0000-0000-0000-000000000019"), "membre1@eeg-grace.cd",      "Joséphine",   "Mbuyi",        AppConstants.Roles.Member,             ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000001A"), "membre2@eeg-grace.cd",      "Emmanuel",    "Tshibola",     AppConstants.Roles.Member,             ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000001B"), "membre3@eeg-grace.cd",      "Élisabeth",   "Ntumba",       AppConstants.Roles.Member,             ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000001C"), "membre4@eeg-grace.cd",      "Bernadette",  "Mukeba",       AppConstants.Roles.Member,             ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000001D"), "membre5@eeg-grace.cd",      "Frédéric",    "Mbombo",       AppConstants.Roles.Member,             ParentChurchId),
        (Guid.Parse("B0000001-0000-0000-0000-00000000001E"), "membre6@eeg-grace.cd",      "Pascaline",   "Mutombo",      AppConstants.Roles.Member,             ChildChurch1Id),
        (Guid.Parse("B0000001-0000-0000-0000-00000000001F"), "membre7@eeg-grace.cd",      "Véronique",   "Bilonda",      AppConstants.Roles.Member,             ChildChurch2Id),
        (Guid.Parse("B0000001-0000-0000-0000-000000000020"), "membre8@eeg-grace.cd",      "Léopold",     "Mukwa",        AppConstants.Roles.Member,             ChildChurch2Id),
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
