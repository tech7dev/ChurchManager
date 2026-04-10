namespace ChurchMS.Shared.Constants;

public static class AuthorizationPolicies
{
    public const string RequireSuperAdmin = nameof(RequireSuperAdmin);
    public const string RequireCentralAdmin = nameof(RequireCentralAdmin);
    public const string RequireChurchAdmin = nameof(RequireChurchAdmin);
    public const string RequireITManager = nameof(RequireITManager);
    public const string RequireSecretary = nameof(RequireSecretary);
    public const string RequireTreasurer = nameof(RequireTreasurer);
    public const string RequireDepartmentHead = nameof(RequireDepartmentHead);
    public const string RequireDepartmentTreasurer = nameof(RequireDepartmentTreasurer);
    public const string RequireTeacher = nameof(RequireTeacher);
    public const string RequireEvangelismLeader = nameof(RequireEvangelismLeader);
    public const string RequireMultimediaManager = nameof(RequireMultimediaManager);
    public const string RequireLogisticsManager = nameof(RequireLogisticsManager);
}
