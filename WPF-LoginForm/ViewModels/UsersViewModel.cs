using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels.Fields;
using WPFBiblioteca.Views.FieldsViews;


namespace WPFBiblioteca.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<UserModel> _collectionUsers;
        private UserModel _usersModelRow;
        private readonly IUserRepository _userRepository;
        private bool CanDelete;
        #endregion

        #region ICommands
        public ICommand GetByAllCommand { get; }
        public ICommand NavigateAddCommand { get; }
        public ICommand RemoveRowCommand { get; }
        #endregion

        #region constructor
        public UsersViewModel(NavigationStore navigationStore)
        {
            CanDelete = false;
            _userRepository = new UserRepository();
            _usersModelRow = new UserModel();
            NavigateAddCommand = new NavigateCommand<UserFieldsViewModel>(new NavigationService<UserFieldsViewModel>(navigationStore, () => new UserFieldsViewModel(navigationStore)));
            GetByAllCommand = new ViewModelCommand(ExecuteGetAllCommand);
            ExecuteGetAllCommand(null);
            RemoveRowCommand = new ViewModelCommand(ExecuteRemoveRowCommand, CanExecuteRemoveRowCommand);
        }



        #endregion

        #region Methods

        private bool CanExecuteRemoveRowCommand(object obj)
        {
            return CanDelete;
        }
        private void ExecuteRemoveRowCommand(object obj)
        {
            
            _userRepository.Remove(_usersModelRow.Id);
            ExecuteGetAllCommand(null);
        }
        private async void ExecuteGetAllCommand(object obj)
        {
             CollectionUser = new ObservableCollection<UserModel>(await _userRepository.GetByAll());

        }
        #endregion

        #region Properties
        public ObservableCollection<UserModel> CollectionUser
        {
            get => _collectionUsers;
            set
            {
                _collectionUsers = value;
                OnPropertyChanged(nameof(CollectionUser));
            }
        }
        public UserModel UsersModelRow
        {
            get => _usersModelRow;
            set
            {
                _usersModelRow = value;
                CanDelete = _usersModelRow != null;
                
                
                OnPropertyChanged(nameof(_usersModelRow));
            }
        }
        #endregion
    }
}
