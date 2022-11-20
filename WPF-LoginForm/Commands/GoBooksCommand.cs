using WPFBiblioteca.Services;
using WPFBiblioteca.ViewModels;

namespace WPFBiblioteca.Commands;

public class GoBooksCommand : CommandBase
{
    private readonly NavigationService<BooksViewModel> _navigationService;
    private readonly BooksViewModel _viewModel;

    public GoBooksCommand(BooksViewModel viewModel, NavigationService<BooksViewModel> navigationService)
    {
        _viewModel = viewModel;
        _navigationService = navigationService;
    }

    public override void Execute(object parameter)
    {
        _navigationService.Navigate();
    }
}