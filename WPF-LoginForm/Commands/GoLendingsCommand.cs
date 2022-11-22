
using WPFBiblioteca.Services;
using WPFBiblioteca.ViewModels;

namespace WPFBiblioteca.Commands
{
    public class GoLendingsCommand : CommandBase
    {
        private readonly NavigationService<LendingsViewModel> _navigationService;
        private readonly LendingsViewModel _viewModel;

        public GoLendingsCommand(LendingsViewModel viewModel, NavigationService<LendingsViewModel> navigationService)
        {
            _viewModel = viewModel;
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
