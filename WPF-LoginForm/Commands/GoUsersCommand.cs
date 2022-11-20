using MySqlConnector;
using WPFBiblioteca.Services;
using WPFBiblioteca.ViewModels;

namespace WPFBiblioteca.Commands;

public class GoUsersCommand : CommandBase
{
    private readonly NavigationService<UsersViewModel> _navigationService;
    private readonly UsersViewModel _viewModel;

    public GoUsersCommand(UsersViewModel viewModel, NavigationService<UsersViewModel> navigationService)
    {
        _viewModel = viewModel;
        _navigationService = navigationService;
    }

    public GoUsersCommand(UsersViewModel viewModel, NavigationService<UsersViewModel> navigationService,
        MySqlException errorCode)
    {
        _viewModel = viewModel;
        _navigationService = navigationService;
    }

    public override void Execute(object parameter)
    {
        _navigationService.Navigate();
    }
}