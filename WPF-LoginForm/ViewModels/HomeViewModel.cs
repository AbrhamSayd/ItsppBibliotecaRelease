using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels.Fields;

namespace WPFBiblioteca.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region fields

        private UserModel _currentUser;
        private NavigationStore _navigationStore;
        private readonly UserRepository _userRepository;

        #endregion

        #region ICommands

        public ICommand NavigateAddCommand { get; }

        #endregion

        #region Methods
        private void LoadCurrentUserData()
        {
            if (Thread.CurrentPrincipal != null)
            {
                var user = _userRepository.GetByUsername(Thread.CurrentPrincipal.Identity!.Name);
                if (user != null)
                {
                    _currentUser = user;
                }
            }
        }
        #endregion


        #region Constructor

        public HomeViewModel(NavigationStore navigationStore, UserModel currentUser)
        {
            _currentUser = currentUser;
            _userRepository = new UserRepository();
            _navigationStore = navigationStore;
            if (currentUser == null)
            {
                LoadCurrentUserData();
            }
            
            
            NavigateAddCommand = new NavigateCommand<LendingsFieldsViewModel>(
                new NavigationService<LendingsFieldsViewModel>(navigationStore,
                    () => new LendingsFieldsViewModel(null, "Add", navigationStore, _currentUser)));
        }

        #endregion
    }
}