using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Common;
using ChurchMS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChurchMS.Persistence;

/// <summary>
/// Application database context with Identity, multi-tenancy, soft-delete, and audit trail.
/// </summary>
public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    ICurrentUserService currentUserService,
    ITenantService tenantService,
    IDateTimeService dateTimeService)
    : IdentityDbContext<AppUser, AppIdentityRole, Guid>(options)
{
    public DbSet<Church> Churches => Set<Church>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Family> Families => Set<Family>();
    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<CustomField> CustomFields => Set<CustomField>();
    public DbSet<MemberCustomFieldValue> MemberCustomFieldValues => Set<MemberCustomFieldValue>();

    // Finance - Contributions
    public DbSet<Fund> Funds => Set<Fund>();
    public DbSet<Contribution> Contributions => Set<Contribution>();
    public DbSet<ContributionCampaign> ContributionCampaigns => Set<ContributionCampaign>();
    public DbSet<Pledge> Pledges => Set<Pledge>();

    // Finance - Accounting
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<AccountTransaction> AccountTransactions => Set<AccountTransaction>();

    // Finance - Expenses
    public DbSet<ExpenseCategory> ExpenseCategories => Set<ExpenseCategory>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<BudgetLine> BudgetLines => Set<BudgetLine>();

    // Events & Attendance
    public DbSet<ChurchEvent> ChurchEvents => Set<ChurchEvent>();
    public DbSet<EventRegistration> EventRegistrations => Set<EventRegistration>();
    public DbSet<EventAttendance> EventAttendances => Set<EventAttendance>();

    // Sunday School
    public DbSet<SundaySchoolClass> SundaySchoolClasses => Set<SundaySchoolClass>();
    public DbSet<SundaySchoolLesson> SundaySchoolLessons => Set<SundaySchoolLesson>();
    public DbSet<SundaySchoolEnrollment> SundaySchoolEnrollments => Set<SundaySchoolEnrollment>();
    public DbSet<SundaySchoolAttendance> SundaySchoolAttendances => Set<SundaySchoolAttendance>();

    // Growth School
    public DbSet<GrowthSchoolCourse> GrowthSchoolCourses => Set<GrowthSchoolCourse>();
    public DbSet<GrowthSchoolSession> GrowthSchoolSessions => Set<GrowthSchoolSession>();
    public DbSet<GrowthSchoolEnrollment> GrowthSchoolEnrollments => Set<GrowthSchoolEnrollment>();
    public DbSet<GrowthSchoolAttendance> GrowthSchoolAttendances => Set<GrowthSchoolAttendance>();

    // Departments
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<DepartmentMember> DepartmentMembers => Set<DepartmentMember>();
    public DbSet<DepartmentTransaction> DepartmentTransactions => Set<DepartmentTransaction>();

    // Messaging & Communication
    public DbSet<MessageCampaign> MessageCampaigns => Set<MessageCampaign>();
    public DbSet<MessageRecipient> MessageRecipients => Set<MessageRecipient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    // Notifications
    public DbSet<Notification> Notifications => Set<Notification>();

    // Secretariat
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<BaptismRecord> BaptismRecords => Set<BaptismRecord>();
    public DbSet<MarriageRecord> MarriageRecords => Set<MarriageRecord>();

    // Evangelism
    public DbSet<EvangelismCampaign> EvangelismCampaigns => Set<EvangelismCampaign>();
    public DbSet<EvangelismTeam> EvangelismTeams => Set<EvangelismTeam>();
    public DbSet<EvangelismTeamMember> EvangelismTeamMembers => Set<EvangelismTeamMember>();
    public DbSet<EvangelismContact> EvangelismContacts => Set<EvangelismContact>();
    public DbSet<EvangelismFollowUp> EvangelismFollowUps => Set<EvangelismFollowUp>();

    // Multimedia
    public DbSet<MediaContent> MediaContents => Set<MediaContent>();
    public DbSet<MediaPurchase> MediaPurchases => Set<MediaPurchase>();
    public DbSet<MediaPromotion> MediaPromotions => Set<MediaPromotion>();

    // Logistics
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VehicleBooking> VehicleBookings => Set<VehicleBooking>();

    // IT Management
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<SupportTicketComment> SupportTicketComments => Set<SupportTicketComment>();
    public DbSet<IntegrationConfig> IntegrationConfigs => Set<IntegrationConfig>();
    public DbSet<SystemLog> SystemLogs => Set<SystemLog>();

    // Subscriptions & Billing
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<SmsCredit> SmsCredits => Set<SmsCredit>();
    public DbSet<SmsCreditTransaction> SmsCreditTransactions => Set<SmsCreditTransaction>();

    // Store tenant ID in a field so EF Core captures it by reference in query filters
    private Guid? TenantChurchId => tenantService.GetCurrentChurchId();
    private bool IsSuperAdmin => tenantService.IsSuperAdmin();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Dynamic query filters (capturing `this.TenantChurchId`) cause EF to think the model
        // changes each build. This is by design for multi-tenancy — suppress the noise.
        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply all IEntityTypeConfiguration from this assembly
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Global query filter: soft-delete for all BaseEntity-derived types
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).HasQueryFilter(
                    CreateSoftDeleteFilter(entityType.ClrType));
            }

            // Multi-tenancy filter for TenantEntity-derived types
            if (typeof(TenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).HasQueryFilter(
                    CreateTenantFilter(entityType.ClrType));
            }
        }

        // Set all FK delete behaviors on domain entities to NoAction.
        // SQL Server forbids multiple CASCADE paths; since we use soft-delete we never cascade hard deletes.
        // Identity tables are excluded — they manage their own cascades.
        var identityTablePrefixes = new[] { "AspNet" };
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            var isIdentityType = identityTablePrefixes.Any(p => clrType.Name.StartsWith(p, StringComparison.OrdinalIgnoreCase));
            if (isIdentityType) continue;

            foreach (var fk in entityType.GetForeignKeys())
            {
                if (fk.DeleteBehavior == DeleteBehavior.Cascade ||
                    fk.DeleteBehavior == DeleteBehavior.SetNull)
                {
                    fk.DeleteBehavior = DeleteBehavior.NoAction;
                }
            }
        }

        // Seed roles
        Seed.RoleSeedData.SeedRoles(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = currentUserService.GetUserId()?.ToString();
        var now = dateTimeService.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = userId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = userId;
                    break;
            }
        }

        // Handle AppUser audit (not BaseEntity but has audit fields)
        foreach (var entry in ChangeTracker.Entries<AppUser>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private System.Linq.Expressions.LambdaExpression CreateSoftDeleteFilter(Type entityType)
    {
        var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "e");
        var isDeletedProperty = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
        var falseConstant = System.Linq.Expressions.Expression.Constant(false);
        var body = System.Linq.Expressions.Expression.Equal(isDeletedProperty, falseConstant);
        return System.Linq.Expressions.Expression.Lambda(body, parameter);
    }

    private System.Linq.Expressions.LambdaExpression CreateTenantFilter(Type entityType)
    {
        // Combined filter: !IsDeleted && (IsSuperAdmin || ChurchId == TenantChurchId)
        var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "e");

        // !IsDeleted
        var isDeletedProperty = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
        var notDeleted = System.Linq.Expressions.Expression.Equal(isDeletedProperty, System.Linq.Expressions.Expression.Constant(false));

        // IsSuperAdmin (reads from context field)
        var isSuperAdminExpr = System.Linq.Expressions.Expression.Property(
            System.Linq.Expressions.Expression.Constant(this), nameof(IsSuperAdmin));

        // ChurchId == TenantChurchId
        var churchIdProperty = System.Linq.Expressions.Expression.Property(parameter, nameof(TenantEntity.ChurchId));
        var tenantIdExpr = System.Linq.Expressions.Expression.Property(
            System.Linq.Expressions.Expression.Constant(this), nameof(TenantChurchId));
        var tenantIdValue = System.Linq.Expressions.Expression.Convert(tenantIdExpr, typeof(Guid));
        var churchIdEquals = System.Linq.Expressions.Expression.Equal(churchIdProperty, tenantIdValue);

        // IsSuperAdmin || ChurchId == TenantChurchId
        var tenantOrSuperAdmin = System.Linq.Expressions.Expression.OrElse(isSuperAdminExpr, churchIdEquals);

        // !IsDeleted && (IsSuperAdmin || ChurchId == TenantChurchId)
        var combined = System.Linq.Expressions.Expression.AndAlso(notDeleted, tenantOrSuperAdmin);

        return System.Linq.Expressions.Expression.Lambda(combined, parameter);
    }
}
