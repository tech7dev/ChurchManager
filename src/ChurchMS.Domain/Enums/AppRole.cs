namespace ChurchMS.Domain.Enums;

/// <summary>
/// Application authorization roles ordered from highest to lowest privilege.
/// </summary>
public enum AppRole
{
    SuperAdmin = 1,
    CentralAdmin = 2,
    ChurchAdmin = 3,
    ITManager = 4,
    Secretary = 5,
    Treasurer = 6,
    DepartmentHead = 7,
    DepartmentTreasurer = 8,
    Teacher = 9,
    EvangelismLeader = 10,
    MultimediaManager = 11,
    LogisticsManager = 12,
    Member = 13
}
