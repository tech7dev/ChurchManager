using ChurchMS.MAUI.Services;

namespace ChurchMS.MAUI;

public partial class App : Application
{
    private readonly IAuthService _authService;

    public App(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell()) { Title = "ChurchMS" };
    }

    protected override async void OnStart()
    {
        base.OnStart();
        var isAuth = await _authService.IsAuthenticatedAsync();
        if (!isAuth)
            await Shell.Current.GoToAsync("//login");
    }
}