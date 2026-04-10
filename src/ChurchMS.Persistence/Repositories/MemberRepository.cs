using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChurchMS.Persistence.Repositories;

public class MemberRepository(AppDbContext context)
    : GenericRepository<Member>(context), IMemberRepository
{
    public async Task<Member?> GetByMembershipNumberAsync(string membershipNumber, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(m => m.MembershipNumber == membershipNumber, cancellationToken);
    }

    public async Task<Member?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(m => m.Family)
            .Include(m => m.CustomFieldValues)
                .ThenInclude(v => v.CustomField)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Member> Items, int TotalCount)> GetPagedAsync(
        string? searchTerm,
        string? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.Include(m => m.Family).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLowerInvariant();
            query = query.Where(m =>
                m.FirstName.ToLower().Contains(term) ||
                m.LastName.ToLower().Contains(term) ||
                m.MembershipNumber.ToLower().Contains(term) ||
                (m.Email != null && m.Email.ToLower().Contains(term)) ||
                (m.Phone != null && m.Phone.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<MemberStatus>(status, true, out var memberStatus))
        {
            query = query.Where(m => m.Status == memberStatus);
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(m => m.LastName).ThenBy(m => m.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<string> GenerateNextMembershipNumberAsync(Guid churchId, CancellationToken cancellationToken = default)
    {
        // Count all members including deleted ones to avoid number reuse
        var count = await Context.Members
            .IgnoreQueryFilters()
            .CountAsync(m => m.ChurchId == churchId, cancellationToken);

        return $"MBR-{(count + 1):D6}";
    }

    public async Task<bool> IsMembershipNumberUniqueAsync(
        string number,
        Guid churchId,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.Where(m => m.ChurchId == churchId && m.MembershipNumber == number);
        if (excludeId.HasValue)
            query = query.Where(m => m.Id != excludeId.Value);

        return !await query.AnyAsync(cancellationToken);
    }
}
