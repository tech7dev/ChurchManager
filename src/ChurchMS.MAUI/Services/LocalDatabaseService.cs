using SQLite;
using ChurchMS.MAUI.Models;

namespace ChurchMS.MAUI.Services;

public interface ILocalDatabaseService
{
    Task InitializeAsync();
    Task<List<LocalMember>> GetMembersAsync();
    Task<int> SaveMemberAsync(LocalMember member);
    Task<int> SaveMembersAsync(IEnumerable<LocalMember> members);
    Task<List<LocalContribution>> GetContributionsAsync();
    Task<int> SaveContributionAsync(LocalContribution contribution);
    Task<List<LocalAttendance>> GetPendingAttendanceAsync();
    Task<int> SaveAttendanceAsync(LocalAttendance attendance);
    Task MarkAttendanceSyncedAsync(Guid id);
}

public class LocalDatabaseService : ILocalDatabaseService
{
    private SQLiteAsyncConnection? _db;

    private async Task<SQLiteAsyncConnection> GetDbAsync()
    {
        if (_db is not null) return _db;
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "churchms.db");
        _db = new SQLiteAsyncConnection(dbPath);
        await _db.CreateTableAsync<LocalMember>();
        await _db.CreateTableAsync<LocalContribution>();
        await _db.CreateTableAsync<LocalAttendance>();
        return _db;
    }

    public async Task InitializeAsync() => await GetDbAsync();

    public async Task<List<LocalMember>> GetMembersAsync()
    {
        var db = await GetDbAsync();
        return await db.Table<LocalMember>().OrderBy(m => m.LastName).ToListAsync();
    }

    public async Task<int> SaveMemberAsync(LocalMember member)
    {
        var db = await GetDbAsync();
        return await db.InsertOrReplaceAsync(member);
    }

    public async Task<int> SaveMembersAsync(IEnumerable<LocalMember> members)
    {
        var db = await GetDbAsync();
        var list = members.ToList();
        await db.RunInTransactionAsync(t =>
        {
            foreach (var m in list) t.InsertOrReplace(m);
        });
        return list.Count;
    }

    public async Task<List<LocalContribution>> GetContributionsAsync()
    {
        var db = await GetDbAsync();
        return await db.Table<LocalContribution>()
            .OrderByDescending(c => c.ContributionDate)
            .ToListAsync();
    }

    public async Task<int> SaveContributionAsync(LocalContribution contribution)
    {
        var db = await GetDbAsync();
        return await db.InsertOrReplaceAsync(contribution);
    }

    public async Task<List<LocalAttendance>> GetPendingAttendanceAsync()
    {
        var db = await GetDbAsync();
        return await db.Table<LocalAttendance>()
            .Where(a => !a.IsSynced)
            .ToListAsync();
    }

    public async Task<int> SaveAttendanceAsync(LocalAttendance attendance)
    {
        var db = await GetDbAsync();
        return await db.InsertOrReplaceAsync(attendance);
    }

    public async Task MarkAttendanceSyncedAsync(Guid id)
    {
        var db = await GetDbAsync();
        var record = await db.Table<LocalAttendance>().FirstOrDefaultAsync(a => a.Id == id);
        if (record is not null)
        {
            record.IsSynced = true;
            await db.UpdateAsync(record);
        }
    }
}
