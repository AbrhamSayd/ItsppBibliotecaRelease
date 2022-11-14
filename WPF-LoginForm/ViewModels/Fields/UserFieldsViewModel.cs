using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;

namespace WPFBiblioteca.ViewModels.Fields
{
    public class UserFieldsViewModel :ViewModelBase
    {
        #region Fields

        private readonly ErrorsViewModel _errorsViewModel;
        private readonly UsersViewModel _viewModel;
        private readonly NavigationService<UsersViewModel> _navigationService;
        private UserModel _userModel;
        private int _id;
        private string _username;
        private SecureString _password;
        private string _firstName;
        private string _lastName;
        private string _userType;
        private readonly IUserRepository _userRepository;
        #endregion

        #region Icommands
        public ICommand AddCommand { get; }
        public ICommand GoBackCommand {get; }
        #endregion

        #region Constructor
        public UserFieldsViewModel(NavigationStore navigationStore)
        {
            GoBackCommand = new GoUsersCommand(_viewModel,
                new NavigationService<UsersViewModel>(navigationStore,
                    () => new UsersViewModel(navigationStore)));
            _userRepository = new UserRepository();
            AddCommand = new ViewModelCommand(ExecuteAddCommand, CanExecuteAddCommand);
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        }
        #endregion

        #region Methods

        private bool CanExecuteAddCommand(object obj)
        {
            return CanCreate;
        }
        private async void ExecuteAddCommand(object obj)
        {
            _userModel = new UserModel()
            {
                Id = _id,
                Username = _username,
                Password = new NetworkCredential("",_password).Password,
                FirstName = _firstName,
                LastName = _lastName,
                UserType = _userType
            };
           await _userRepository.Add(_userModel);
            GoBackCommand.Execute(null);
        }
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion

        #region Properties
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                _errorsViewModel.ClearErrors(nameof(Id));
                if (Math.Floor(Math.Log10(_id) + 1)
                is < 3 or > 8)
                {
                    _errorsViewModel.AddError(nameof(Id), "Numero de empleado invalido");
                }
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                _errorsViewModel.ClearErrors(nameof(Username));
                if (string.IsNullOrWhiteSpace(_username) || _username.Length < 3)
                {
                    _errorsViewModel.AddError(nameof(Username), "Nombre de usuario invalido");
                }
                OnPropertyChanged(nameof(Username));
            }
        }
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                _errorsViewModel.ClearErrors(nameof(Password));
                if (_password == null || _password.Length < 3)
                {
                    _errorsViewModel.AddError(nameof(Password), "Contraseña incorrecta");
                }
                OnPropertyChanged(nameof(Password));
            }
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                _errorsViewModel.ClearErrors(nameof(FirstName));
                if (string.IsNullOrEmpty(_firstName) || _firstName.Length <= 1)
                {
                    _errorsViewModel.AddError(nameof(FirstName), "Nombre invalido");
                }
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                _errorsViewModel.ClearErrors(nameof(LastName));
                if (string.IsNullOrEmpty(_lastName) || _lastName.Length < 3)
                    _errorsViewModel.AddError(nameof(LastName), "Apellido invalido");
                
                OnPropertyChanged(nameof(LastName));
            }
        }
        
        public string UserType
        {
            get => _userType;
            set
            {
                _userType = value;
                _errorsViewModel.ClearErrors(nameof(UserType));
                if (string.IsNullOrWhiteSpace(_userType))
                    _errorsViewModel.AddError(nameof(UserType), "Tipo de usuario invalido");
                OnPropertyChanged(nameof(UserType));
            }
        }
        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;
        #endregion



    }
}
