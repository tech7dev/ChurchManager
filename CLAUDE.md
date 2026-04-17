# ChurchMS — Church Management Solution

Multi-tenant church management platform: ASP.NET Core Web API + Blazor Server admin + .NET MAUI mobile (Android/iOS).

See @README.md for setup instructions.
See @docs/functional-spec-en.md for full functional specification.
See @docs/functional-spec-fr.md for French specification.

## Tech Stack

- **Runtime:** .NET 10
- **API:** ASP.NET Core 10 Web API (REST, versioned `/api/v1/`)
- **Web Admin:** Blazor Server 10 + MudBlazor
- **Mobile:** .NET MAUI 10 (Android + iOS)
- **Database:** SQL Server 2022 + Entity Framework Core 10 (Code-First)
- **Cache:** Redis 7
- **Auth:** ASP.NET Identity + JWT Bearer + Refresh Tokens
- **Messaging:** MediatR (CQRS), SignalR (real-time)
- **Validation:** FluentValidation
- **Mapping:** Mapster
- **Logging:** Serilog → Seq
- **Payments:** Stripe, PayPal, Flutterwave (Mobile Money)
- **SMS/Email:** Twilio, SendGrid, Africa's Talking, WhatsApp Business API
- **Storage:** Azure Blob Storage / MinIO
- **Containers:** Docker + Docker Compose
- **IDE:** Visual Studio Code

## Solution Structure

```
ChurchMS.sln
├── src/
│   ├── ChurchMS.Domain/              # Entities, enums, domain events, interfaces
│   ├── ChurchMS.Application/         # CQRS handlers, DTOs, validators, service interfaces
│   │   └── Features/                 # One folder per module (see Module Map below)
│   ├── ChurchMS.Infrastructure/      # External services: payments, SMS, email, storage
│   ├── ChurchMS.Persistence/         # EF Core DbContext, configurations, migrations, repositories
│   ├── ChurchMS.API/                 # Controllers, middleware, SignalR hubs, filters
│   ├── ChurchMS.BlazorAdmin/         # Blazor Server admin dashboard (MudBlazor)
│   ├── ChurchMS.MAUI/               # .NET MAUI mobile app (MVVM, offline SQLite)
│   └── ChurchMS.Shared/             # Shared DTOs, enums, constants across all projects
├── tests/
│   ├── ChurchMS.UnitTests/
│   ├── ChurchMS.IntegrationTests/
│   └── ChurchMS.API.Tests/
├── docker/
│   └── docker-compose.yml
└── docs/
    ├── functional-spec-en.md
    └── functional-spec-fr.md
```

## Architecture Rules

- **Clean Architecture (Onion):** Domain → Application → Infrastructure/Persistence → API. Dependencies always point inward.
- **CQRS with MediatR:** Every use case is a Command or Query in `Application/Features/{Module}/`. No business logic in controllers.
- **Multi-tenancy:** Shared database, `ChurchId` discriminator on all tenant entities. EF Core global query filters enforce isolation automatically. Tenant resolved from JWT `churchId` claim via `ITenantService`.
- **Church hierarchy:** Self-referencing `ParentChurchId`. Parent sees all descendant data. Child churches operate independently.
- **Soft delete:** All entities have `IsDeleted` flag. Never hard-delete. Global query filter excludes deleted records.
- **Audit trail:** `CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy` set automatically in `DbContext.SaveChangesAsync()`.
- **Repository pattern:** Generic `IRepository<T>` + `IUnitOfWork`. Specialized repos only when complex queries require it.

## Module Map

Each functional module maps to a feature folder in `Application/Features/` and a controller in `API/Controllers/`. **All 15 modules must be maintained — do not merge or skip any:**

| # | Module | Feature Folder | Controller | Spec Section |
|---|--------|---------------|------------|-------------|
| 1 | Auth & Identity | `Auth/` | `AuthController` | §1 (Roles) |
| 2 | Churches & Setup | `Churches/` | `ChurchesController` | §2 (Onboarding) |
| 3 | Subscriptions & Billing | `Subscriptions/` | `SubscriptionsController` | §3 |
| 4 | Finance (Contributions) | `Contributions/` | `ContributionsController` | §4.1 |
| 5 | Finance (Accounting) | `Accounting/` | `AccountingController` | §4.2 |
| 6 | Finance (Expenses & Budgets) | `Expenses/` | `ExpensesController` | §4.3 |
| 7 | Members & People | `Members/` | `MembersController` | §5.1–5.9 |
| 8 | Communication & Messaging | `Messaging/` | `MessagingController` | §5.3 |
| 9 | Events & Attendance | `Events/` | `EventsController` | §6 |
| 10 | Sunday School | `SundaySchool/` | `SundaySchoolController` | §7 |
| 11 | Growth School | `GrowthSchool/` | `GrowthSchoolController` | §8 |
| 12 | Administration | `Administration/` | shared with other controllers | §9 |
| 13 | Secretariat | `Secretariat/` | `SecretariatController` | §10 |
| 14 | Dept. Treasury & Head | `Departments/` | `DepartmentsController` | §11–12 |
| 15 | Evangelism | `Evangelism/` | `EvangelismController` | §13 |
| 16 | Member App features | consumed via existing APIs | — (MAUI app) | §14 |
| 17 | Multimedia | `Multimedia/` | `MultimediaController` | §15 |
| 18 | Logistics | `Logistics/` | `LogisticsController` | §16 |
| 19 | IT Management | `ITManagement/` | `ITManagementController` | §17 |
| 20 | Reports | `Reports/` | `ReportsController` | cross-module |
| 21 | Notifications | `Notifications/` | `NotificationsController` | cross-module |

## Commands

```bash
# Restore & build
dotnet restore ChurchMS.sln
dotnet build ChurchMS.sln

# Run API (from src/ChurchMS.API/)
dotnet run --project src/ChurchMS.API/ChurchMS.API.csproj

# Run Blazor Admin (from src/ChurchMS.BlazorAdmin/)
dotnet run --project src/ChurchMS.BlazorAdmin/ChurchMS.BlazorAdmin.csproj

# Run MAUI (from src/ChurchMS.MAUI/)
dotnet build src/ChurchMS.MAUI/ChurchMS.MAUI.csproj -f net10.0-android
dotnet build src/ChurchMS.MAUI/ChurchMS.MAUI.csproj -f net10.0-ios

# EF Core migrations (run from solution root)
dotnet ef migrations add <MigrationName> --project src/ChurchMS.Persistence --startup-project src/ChurchMS.API
dotnet ef database update --project src/ChurchMS.Persistence --startup-project src/ChurchMS.API

# Tests
dotnet test tests/ChurchMS.UnitTests/
dotnet test tests/ChurchMS.IntegrationTests/
dotnet test  # runs all tests

# Docker infrastructure
cd docker && docker-compose up -d          # start SQL Server, Redis, Seq
cd docker && docker-compose down           # stop all

# Linting & formatting
dotnet format ChurchMS.sln
```

## Database (SQL Server)

- **Connection string name:** `DefaultConnection`
- **Provider:** `Microsoft.EntityFrameworkCore.SqlServer`
- **Schema convention:** default `dbo` schema
- **Table naming:** entity class name pluralized (EF convention)
- **All monetary fields:** `decimal(18,2)` precision
- **All string PKs/FKs:** `Guid` (no auto-increment integers)
- **Indexes required on:** `ChurchId` (every tenant table), `Date` columns used in filters, composite `(ChurchId, MembershipNumber)`
- SQL Server collation: `Latin1_General_100_CI_AS_SC_UTF8` (for multilingual support)

## Entity Base Classes

```csharp
// Every entity inherits from this
public abstract class BaseEntity {
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}

// Every church-scoped entity inherits from this
public abstract class TenantEntity : BaseEntity {
    public Guid ChurchId { get; set; }
}
```

## Code Style (C#)

- 4-space indentation
- `PascalCase` for classes, methods, properties, public fields
- `_camelCase` for private fields
- `camelCase` for local variables, parameters
- File-scoped namespaces (`namespace X;`)
- Primary constructors where appropriate
- Nullable reference types enabled (`<Nullable>enable</Nullable>`)
- XML doc comments on all public APIs
- One class per file (exceptions: small related records/enums)

## CQRS Pattern (per feature)

```
Application/Features/{Module}/
├── Commands/
│   ├── Create{Entity}/
│   │   ├── Create{Entity}Command.cs       # : IRequest<{Response}>
│   │   ├── Create{Entity}CommandHandler.cs # : IRequestHandler<...>
│   │   └── Create{Entity}Validator.cs      # : AbstractValidator<...>
│   └── Update{Entity}/
│       └── ...
└── Queries/
    ├── Get{Entity}ById/
    │   ├── Get{Entity}ByIdQuery.cs
    │   └── Get{Entity}ByIdQueryHandler.cs
    └── Get{Entity}List/
        └── ...
```

## API Conventions

- Route pattern: `[Route("api/v1/[controller]")]`
- All controllers inherit `BaseApiController` (provides `Mediator` property)
- Standardized response envelope: `ApiResponse<T>` with `Success`, `Data`, `Message`, `Errors`
- Pagination: `PagedResult<T>` with `Items`, `TotalCount`, `Page`, `PageSize`
- Authorization via `[Authorize(Policy = "...")]` attributes
- Naming: controllers are plural nouns (`MembersController`, `EventsController`)
- HTTP verbs: GET (read), POST (create), PUT (update), DELETE (soft-delete)
- Return `201 Created` for POST, `200 OK` for GET/PUT, `204 No Content` for DELETE

## Authorization Roles

13 roles with hierarchical access (highest to lowest):

1. `SuperAdmin` — platform-wide, all churches
2. `CentralAdmin` — parent + all descendant churches
3. `ChurchAdmin` — single church, all modules
4. `ITManager` — users, security, system, single church
5. `Secretary` — members, docs, communication, audiences, certificates
6. `Treasurer` — full financial access for single church
7. `DepartmentHead` — scoped to their department
8. `DepartmentTreasurer` — department finances only
9. `Teacher` — classes, lessons, evaluations
10. `EvangelismLeader` — evangelism module
11. `MultimediaManager` — multimedia content
12. `LogisticsManager` — inventory, vehicles
13. `Member` — personal scope only (mobile app)

## MAUI App Specifics

- **Architecture:** MVVM with CommunityToolkit.Mvvm
- **Local DB:** SQLite via sqlite-net-pcl for offline support
- **Sync:** Delta sync with `LastModified` timestamps, background service on reconnect
- **Navigation:** Shell-based navigation
- **DI:** Microsoft.Extensions.DependencyInjection
- **HTTP:** HttpClient with Refit for typed API calls
- **QR scanning:** ZXing.Net.Maui

## Blazor Admin Specifics

- **UI Framework:** MudBlazor (do not mix with other component libraries)
- **State management:** Cascading parameters for auth state, Fluxor for complex state
- **API calls:** Typed HttpClient services injected via DI
- **Authentication:** Cookie-based with JWT passed to API in Authorization header
- **Pages organized by module:** `Pages/{ModuleName}/`

## Key Business Rules

1. **Max 5 bank accounts** per church
2. **Mobile money operators** depend on the church's country
3. **Dual currency:** every church has a primary and optional secondary currency
4. **Church hierarchy:** parent can see descendants, child cannot see parent's data
5. **Subscription required:** each church (including child churches) must have an active subscription
6. **QR codes auto-generated** for: churches (app download), members (ID card), events (sharing), registrations (check-in)
7. **Offline mode (MAUI):** attendance, contributions, and notes work offline and queue for sync
8. **Certificates:** baptism, marriage, member cards, school report cards — all use church-customizable templates
9. **SMS credits:** purchased separately, consumed per message sent
10. **Audience/appointment system:** request → schedule → confirm → remind (10 min + 5 min before) → complete
11. **Video call appointments:** member requests, responsible confirms or reschedules, notifications sent before
12. **Multimedia purchases:** can be paid online OR cash at cashier (staff manually activates content)
13. **Expense approval workflow:** submit → approve/reject by authorized role → mark as paid
14. **Recurring contributions:** auto-processed on schedule via background job (Hangfire)

## Testing Strategy

- **Unit tests:** for all command/query handlers, validators, domain logic. Use xUnit + Moq/NSubstitute.
- **Integration tests:** for repository queries and API endpoints. Use WebApplicationFactory + Testcontainers (SQL Server).
- **Test naming:** `MethodName_Scenario_ExpectedResult` (e.g., `CreateMember_WithValidData_ReturnsMemberDto`)
- **Arrange/Act/Assert** pattern with commented sections.
- Minimum coverage target: 80% on Application layer.

## Git Conventions

- **Branch naming:** `feature/{module}-{description}`, `fix/{module}-{description}`, `chore/{description}`
- **Commit format:** Conventional Commits — `feat(members):`, `fix(finance):`, `refactor(api):`, `test(events):`, `docs:`, `chore:`
- **Scopes:** `auth`, `churches`, `subscriptions`, `contributions`, `accounting`, `expenses`, `members`, `messaging`, `events`, `sunday-school`, `growth-school`, `secretariat`, `departments`, `evangelism`, `multimedia`, `logistics`, `it-management`, `reports`, `notifications`, `maui`, `blazor`, `api`, `infra`, `docker`
- Never commit directly to `main`. Always use feature branches + pull requests.
- Run `dotnet format` and `dotnet test` before pushing.

## Implementation Order (Phased)

When building features, follow this dependency order:

| # | Phase | Status | Notes |
| --- | ----- | ------ | ----- |
| 1 | **Foundation** | ✅ COMPLETE | Domain entities, DbContext, Identity+JWT auth, CQRS pipeline, repos, global query filters, multi-tenancy, exception middleware, AuthController, ChurchesController |
| 2 | **Members** | ✅ COMPLETE | Member/Family/Visitor/CustomField entities + full CRUD CQRS + MembersController + VisitorsController |
| 3 | **Finance Core** | ✅ COMPLETE | 10 entities (Fund, Contribution, ContributionCampaign, Pledge, BankAccount, AccountTransaction, ExpenseCategory, Expense, Budget, BudgetLine); ContributionsController, AccountingController, ExpensesController |
| 4 | **Events** | ✅ COMPLETE | ChurchEvent, EventRegistration, EventAttendance + CQRS + EventsController |
| 5 | **Schools** | ✅ COMPLETE | Sunday School (SundaySchoolClass, Lesson, Enrollment, Attendance) + CQRS + SundaySchoolController. Growth School (GrowthSchoolCourse, GrowthSchoolSession, GrowthSchoolEnrollment, GrowthSchoolAttendance) + GrowthSchoolLevel enum + CQRS (CreateCourse, CreateSession, EnrollMember, RecordAttendance, GetCourseList, GetSessionList) + GrowthSchoolController + AddGrowthSchool migration. |
| 6 | **Departments** | ✅ COMPLETE | Department, DepartmentMember, DepartmentTransaction + CQRS + DepartmentsController |
| 7 | **Communication** | ✅ COMPLETE | MessageCampaign, MessageRecipient, Appointment, Notification + CQRS + MessagingController + NotificationsController |
| 8 | **Secretariat** | ✅ COMPLETE | Document, Certificate, BaptismRecord, MarriageRecord entities + CQRS (RegisterDocument, IssueCertificate, RecordBaptism, RecordMarriage + list queries) + SecretariatController |
| 9 | **Evangelism** | ✅ COMPLETE | EvangelismCampaign, EvangelismTeam, EvangelismTeamMember, EvangelismContact, EvangelismFollowUp entities + CQRS (CreateCampaign, CreateTeam, AssignTeamMember, AddContact, UpdateContactStatus, RecordFollowUp + list queries) + EvangelismController |
| 10 | **Multimedia** | ✅ COMPLETE | MediaContent, MediaPurchase, MediaPromotion entities + CQRS (CreateMediaContent, PublishMediaContent, PurchaseMediaContent, ActivateMediaAccess, CreatePromotion + list/detail queries) + MultimediaController |
| 11 | **Logistics** | ✅ COMPLETE | InventoryItem, InventoryTransaction, Vehicle, VehicleBooking entities + CQRS (CreateInventoryItem, RecordInventoryTransaction, CreateVehicle, BookVehicle, ApproveVehicleBooking + list queries) + LogisticsController |
| 12 | **IT Management** | ✅ COMPLETE | SupportTicket, SupportTicketComment, IntegrationConfig, SystemLog entities + CQRS (CreateSupportTicket, UpdateTicketStatus, AddTicketComment, CreateIntegrationConfig, UpdateIntegrationConfig, UpdateUserStatus, AssignUserRole + list/detail queries) + IUserService/UserService + ITManagementController |
| 13 | **Subscriptions** | ✅ COMPLETE | Subscription, Invoice, SmsCredit, SmsCreditTransaction entities + 5 enums (SubscriptionStatus, InvoiceStatus, PaymentMethod, SmsCreditTransactionType, BillingCycle) + CQRS (CreateSubscription, RenewSubscription, CancelSubscription, ChangePlan, MarkInvoicePaid, PurchaseSmsCredits + list/detail/balance queries) + SubscriptionsController |
| 14 | **Reports** | ✅ COMPLETE | Query-only, no new entities/migration. Dashboard (KPIs), FinancialSummary (contributions vs expenses by fund/category/month), MemberReport (demographics, age groups, growth trend), AttendanceReport (per-event + monthly trend), ContributionReport (paginated detail), ExpenseReport (paginated detail) + ReportsController. Note: ChurchEvent uses StartDateTime (not StartDate), Contribution uses ContributionDate (DateOnly), Expense uses ExpenseDate (DateOnly) + CategoryId + VendorName. |
| 15 | **Blazor Admin** | ✅ COMPLETE | MudBlazor 9.x UI: JWT auth (JwtAuthenticationStateProvider + ProtectedLocalStorage), AuthorizeRouteView + RedirectToLogin, named HttpClient services per module (MemberService, EventService, FinanceService, ReportService, DepartmentService, MessagingService, SubscriptionService, ITManagementService), MudLayout+AppBar+Drawer shell, full NavMenu (all 21 modules), Login page, Dashboard (KPI cards), MemberList+MemberDetail, EventList, ContributionList, ExpenseList, FinancialReport, AttendanceReport, MemberReport pages. BlazorAdmin → Application project reference for DTO reuse. |
| 16 | **MAUI App** | ✅ COMPLETE | CommunityToolkit.Mvvm + SQLite (sqlite-net-pcl) + ZXing.Net.Maui + HttpClient. Shell navigation (login → tabs: Dashboard/Members/Giving/Events). JwtAuthService (SecureStorage), ApiService (HttpClient wrapper), LocalDatabaseService (SQLite CRUD), SyncService (offline→API sync). BaseViewModel, LoginViewModel, DashboardViewModel, MembersViewModel, GivingViewModel, EventsViewModel. LoginPage, DashboardPage (KPI cards, refresh), MembersPage (search + list), GivingPage (submit contribution + recent list), EventsPage (event list + QR scan trigger), QrScannerPage (ZXing camera). NotNullConverter registered in App.xaml. |

### Database

- `InitialCreate` + `AddDepartmentsAndMessaging` + `AddSecretariat` + `AddEvangelism` + `AddMultimedia` + `AddLogistics` + `AddITManagement` + `AddSubscriptions` + `AddGrowthSchool` migrations deployed to remote SQL Server at `84.247.187.33,1433`
- Phases 1–13 + Growth School (Phase 5 completion) deployed
- `OnConfiguring` in `AppDbContext` suppresses `PendingModelChangesWarning` (caused by dynamic multi-tenancy query filters — by design)

## Do Not

- Do NOT hard-delete any records — always soft-delete via `IsDeleted`
- Do NOT put business logic in controllers — use MediatR handlers
- Do NOT bypass tenant filtering — always go through repository/DbContext
- Do NOT use `WidthType.PERCENTAGE` in any document generation (breaks rendering)
- Do NOT store secrets in `appsettings.json` for production — use User Secrets or Azure Key Vault
- Do NOT create database views or stored procedures — keep all logic in EF Core / C#
- Do NOT add NuGet packages without confirming .NET 10 compatibility first
