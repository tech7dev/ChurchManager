using ChurchMS.Domain.Entities;

namespace ChurchMS.Domain.Interfaces;

/// <summary>
/// Specialized repository for Member entities.
/// </summary>
public interface IMemberRepository : IRepository<Member>
{
    Task<Member?> GetByMembershipNumberAsync(string membershipNumber, CancellationToken cancellationToken = default);
    Task<Member?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Member> Items, int TotalCount)> GetPagedAsync(string? searchTerm, string? status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<string> GenerateNextMembershipNumberAsync(Guid churchId, CancellationToken cancellationToken = default);
    Task<bool> IsMembershipNumberUniqueAsync(string number, Guid churchId, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
