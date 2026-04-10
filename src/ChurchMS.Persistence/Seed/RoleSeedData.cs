using ChurchMS.Domain.Entities;
using ChurchMS.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace ChurchMS.Persistence.Seed;

public static class RoleSeedData
{
    public static void SeedRoles(ModelBuilder builder)
    {
        var roles = new[]
        {
            CreateRole(AppConstants.Roles.SuperAdmin, "Platform-wide administrator"),
            CreateRole(AppConstants.Roles.CentralAdmin, "Parent + descendant churches admin"),
            CreateRole(AppConstants.Roles.ChurchAdmin, "Single church full admin"),
            CreateRole(AppConstants.Roles.ITManager, "Users, security, system admin"),
            CreateRole(AppConstants.Roles.Secretary, "Members, docs, communication"),
            CreateRole(AppConstants.Roles.Treasurer, "Full financial access"),
            CreateRole(AppConstants.Roles.DepartmentHead, "Department scope admin"),
            CreateRole(AppConstants.Roles.DepartmentTreasurer, "Department finances"),
            CreateRole(AppConstants.Roles.Teacher, "Classes, lessons, evaluations"),
            CreateRole(AppConstants.Roles.EvangelismLeader, "Evangelism module"),
            CreateRole(AppConstants.Roles.MultimediaManager, "Multimedia content"),
            CreateRole(AppConstants.Roles.LogisticsManager, "Inventory, vehicles"),
            CreateRole(AppConstants.Roles.Member, "Personal scope only"),
        };

        builder.Entity<AppIdentityRole>().HasData(roles);
    }

    private static AppIdentityRole CreateRole(string name, string description) => new(name, description)
    {
        Id = GenerateDeterministicGuid(name),
        NormalizedName = name.ToUpperInvariant()
    };

    /// <summary>
    /// Generates a deterministic GUID from a string so seed data is idempotent.
    /// </summary>
    private static Guid GenerateDeterministicGuid(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = System.Security.Cryptography.SHA256.HashData(bytes);
        return new Guid(hash.AsSpan()[..16]);
    }
}
