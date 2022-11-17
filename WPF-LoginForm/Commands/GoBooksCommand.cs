using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBiblioteca.Services;
using WPFBiblioteca.ViewModels;

namespace WPFBiblioteca.Commands
{
    public class GoBooksCommand : CommandBase
    {
        private readonly BooksViewModel _viewModel;
        private readonly NavigationService<BooksViewModel> _navigationService;

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
}
