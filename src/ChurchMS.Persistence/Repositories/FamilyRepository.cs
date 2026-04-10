using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChurchMS.Persistence.Repositories;

public class FamilyRepository(AppDbContext context)
    : GenericRepository<Family>(context), IFamilyRepository
{
    public async Task<Family?> GetWithMembersAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(f => f.Members.Where(m => !m.IsDeleted))
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }
}
