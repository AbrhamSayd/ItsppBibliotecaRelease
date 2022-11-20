using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using WPFBiblioteca.Services;
using WPFBiblioteca.ViewModels;
using WPFBiblioteca.ViewModels.Fields;

namespace WPFBiblioteca.Commands
{
    public class GoUsersCommand : CommandBase
    {
        private readonly UsersViewModel _viewModel;
        private readonly NavigationService<UsersViewModel> _navigationService;

        public GoUsersCommand(UsersViewModel viewModel, NavigationService<UsersViewModel> navigationService)
        {
            _viewModel = viewModel;
            _navigationService = navigationService;
        }
        public GoUsersCommand(UsersViewModel viewModel, NavigationService<UsersViewModel> navigationService, MySqlException errorCode)
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
