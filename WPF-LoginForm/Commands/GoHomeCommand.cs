using WPFBiblioteca.Services;
using WPFBiblioteca.ViewModels;

namespace WPFBiblioteca.Commands;

public class GoHomeCommand : CommandBase
{
    private readonly NavigationService<HomeViewModel> _navigationService;
    private readonly HomeViewModel _viewModel;

    public GoHomeCommand(HomeViewModel viewModel, NavigationService<HomeViewModel> navigationService)
    {
        _viewModel = viewModel;
        _navigationService = navigationService;
    }

    public override void Execute(object parameter)
    {
        _navigationService.Navigate();
    }
}