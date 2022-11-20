using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBiblioteca.Services;
using WPFBiblioteca.ViewModels;

namespace WPFBiblioteca.Commands
{
    public class GoMembersCommand : CommandBase
    {
        private readonly MembersViewModel _viewModel;
        private readonly NavigationService<MembersViewModel> _navigationService;

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
}
