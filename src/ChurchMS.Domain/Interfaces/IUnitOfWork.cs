namespace ChurchMS.Domain.Interfaces;

/// <summary>
/// Unit of Work pattern to coordinate saving changes across repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
