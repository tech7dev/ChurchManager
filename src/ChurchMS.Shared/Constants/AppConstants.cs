namespace ChurchMS.Shared.Constants;

public static class AppConstants
{
    public const int MaxBankAccountsPerChurch = 5;
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 100;

    public static class Roles
    {
        public const string SuperAdmin = nameof(SuperAdmin);
        public const string CentralAdmin = nameof(CentralAdmin);
        public const string ChurchAdmin = nameof(ChurchAdmin);
        public const string ITManager = nameof(ITManager);
        public const string Secretary = nameof(Secretary);
        public const string Treasurer = nameof(Treasurer);
        public const string DepartmentHead = nameof(DepartmentHead);
        public const string DepartmentTreasurer = nameof(DepartmentTreasurer);
        public const string Teacher = nameof(Teacher);
        public const string EvangelismLeader = nameof(EvangelismLeader);
        public const string MultimediaManager = nameof(MultimediaManager);
        public const string LogisticsManager = nameof(LogisticsManager);
        public const string Member = nameof(Member);
    }
}
