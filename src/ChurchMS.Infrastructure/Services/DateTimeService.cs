using ChurchMS.Application.Interfaces;

namespace ChurchMS.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
