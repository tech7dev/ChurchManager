using ChurchMS.Domain.Entities;

namespace ChurchMS.Domain.Interfaces;

/// <summary>
/// Specialized repository for Family entities.
/// </summary>
public interface IFamilyRepository : IRepository<Family>
{
    Task<Family?> GetWithMembersAsync(Guid id, CancellationToken cancellationToken = default);
}
