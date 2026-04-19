using ChurchMS.BlazorAdmin.Auth;
using ChurchMS.BlazorAdmin.Components;
using ChurchMS.BlazorAdmin.Services;
using ChurchMS.Shared.Constants;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

using Roles = ChurchMS.Shared.Constants.AppConstants.Roles;

var builder = WebApplication.CreateBuilder(args);

// MudBlazor
builder.Services.AddMudServices();

// Fluxor (state management)
builder.Services.AddFluxor(options => options
    .ScanAssemblies(typeof(Program).Assembly)
    .UseRouting());

// Blazor + auth
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAuthentication().AddCookie();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore(options =>
{
    // Policies mirror the API side so [Authorize(Policy = "...")] can be used interchangeably.
    options.AddPolicy(AuthorizationPolicies.RequireSuperAdmin,
        p => p.RequireRole(Roles.SuperAdmin));
    options.AddPolicy(AuthorizationPolicies.RequireCentralAdmin,
        p => p.RequireRole(Roles.SuperAdmin, Roles.CentralAdmin));
    options.AddPolicy(AuthorizationPolicies.RequireChurchAdmin,
        p => p.RequireRole(Roles.SuperAdmin, Roles.CentralAdmin, Roles.ChurchAdmin));
    options.AddPolicy(AuthorizationPolicies.RequireITManager,
        p => p.RequireRole(Roles.SuperAdmin, Roles.ChurchAdmin, Roles.ITManager));
    options.AddPolicy(AuthorizationPolicies.RequireSecretary,
        p => p.RequireRole(Roles.SuperAdmin, Roles.ChurchAdmin, Roles.Secretary));
    options.AddPolicy(AuthorizationPolicies.RequireTreasurer,
        p => p.RequireRole(Roles.SuperAdmin, Roles.ChurchAdmin, Roles.Treasurer));
    options.AddPolicy(AuthorizationPolicies.RequireDepartmentHead,
        p => p.RequireRole(Roles.SuperAdmin, Roles.ChurchAdmin, Roles.DepartmentHead));
    options.AddPolicy(AuthorizationPolicies.RequireDepartmentTreasurer,
        p => p.RequireRole(Roles.SuperAdmin, Roles.ChurchAdmin, Roles.Treasurer, Roles.DepartmentTreasurer));
    options.AddPolicy(AuthorizationPolicies.RequireTeacher,
        p => p.RequireRole(Roles.SuperAdmin, Roles.ChurchAdmin, Roles.Teacher));
    options.AddPolicy(AuthorizationPolicies.RequireEvangelismLeader,
        p => p.RequireRole(Roles.SuperAdmin, Roles.ChurchAdmin, Roles.EvangelismLeader));
    options.AddPolicy(AuthorizationPolicies.RequireMultimediaManager,
        p => p.RequireRole(Roles.SuperAdmin, Roles.ChurchAdmin, Roles.MultimediaManager));
    options.AddPolicy(AuthorizationPolicies.RequireLogisticsManager,
        p => p.RequireRole(Roles.SuperAdmin, Roles.ChurchAdmin, Roles.LogisticsManager));
});
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());

// Auth service
builder.Services.AddScoped<IAuthService, AuthService>();

// Named HttpClient for the API
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7110";
builder.Services.AddHttpClient("ChurchMSApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Module services
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<FinanceService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<MessagingService>();
builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddScoped<ITManagementService>();
builder.Services.AddScoped<EducationService>();
builder.Services.AddScoped<MinistryService>();
builder.Services.AddScoped<LocalisationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

app.Run();
