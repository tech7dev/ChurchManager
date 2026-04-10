using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChurchMS.Persistence.Repositories;

public class ChurchRepository(AppDbContext context)
    : GenericRepository<Church>(context), IChurchRepository
{
    public async Task<Church?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.ChildChurches)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Church?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(c => c.Code == code, cancellationToken);
    }

    public async Task<IReadOnlyList<Church>> GetDescendantsAsync(Guid parentId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(c => c.ParentChurchId == parentId)
            .ToListAsync(cancellationToken);
    }
}
