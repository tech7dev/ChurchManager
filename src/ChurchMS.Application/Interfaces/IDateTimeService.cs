namespace ChurchMS.Application.Interfaces;

/// <summary>
/// Abstraction over DateTime for testability.
/// </summary>
public interface IDateTimeService
{
    DateTime UtcNow { get; }
}
