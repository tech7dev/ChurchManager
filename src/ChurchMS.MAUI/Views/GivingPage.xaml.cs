using ChurchMS.MAUI.ViewModels;

namespace ChurchMS.MAUI.Views;

public partial class GivingPage : ContentPage
{
    private readonly GivingViewModel _viewModel;

    public GivingPage(GivingViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }
}
