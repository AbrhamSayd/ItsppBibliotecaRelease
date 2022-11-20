using WPFBiblioteca.Services;
using WPFBiblioteca.ViewModels;

namespace WPFBiblioteca.Commands;

public class GoMembersCommand : CommandBase
{
    private readonly NavigationService<MembersViewModel> _navigationService;
    private readonly MembersViewModel _viewModel;

    public GoMembersCommand(MembersViewModel viewModel, NavigationService<MembersViewModel> navigationService)
    {
        _viewModel = viewModel;
        _navigationService = navigationService;
    }

    public override void Execute(object parameter)
    {
        _navigationService.Navigate();
    }
}