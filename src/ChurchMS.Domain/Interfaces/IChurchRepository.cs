using ChurchMS.Domain.Entities;

namespace ChurchMS.Domain.Interfaces;

/// <summary>
/// Specialized repository for Church entities.
/// </summary>
public interface IChurchRepository : IRepository<Church>
{
    Task<Church?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Church?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Church>> GetDescendantsAsync(Guid parentId, CancellationToken cancellationToken = default);
}
