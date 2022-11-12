using System;
using System.Collections.Generic;
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
        //Fields
        private readonly UsersViewModel _viewModel;
        private readonly NavigationService<UsersViewModel> _navigationService;
        private UserModel _userModel;
        private int _id;
        private string _username;
        private SecureString _password;
        private string _name;
        private string _lastName;
        private string _userType;
        private readonly IUserRepository _userRepository;

        //ICommands
        public ICommand AddCommand { get; }
        public ICommand GoBackCommand {get; }
        //constructor
        public UserFieldsViewModel(NavigationStore navigationStore)
        {
            GoBackCommand = new GoUsersCommand(_viewModel,
                new NavigationService<UsersViewModel>(navigationStore,
                    () => new UsersViewModel(navigationStore)));

            _userRepository = new UserRepository();

            AddCommand = new ViewModelCommand(ExecuteAddCommand, CanExecuteAddCommand);
        }



        //methods

        private bool CanExecuteAddCommand(object obj)
        {
            bool validData;
            if (Id.ToString().Length < 3 || string.IsNullOrWhiteSpace(Username) || Username.Length < 3 || Password == null || Password.Length < 3 || string.IsNullOrWhiteSpace(Name) || Name.Length <= 1 || string.IsNullOrWhiteSpace(LastName) || LastName.Length < 3 || string.IsNullOrWhiteSpace(UserType))
                validData = false;
            else
                validData = true;
            return validData;


        }
        private void ExecuteAddCommand(object obj)
        {
            _userModel = new UserModel()
            {
                Id = _id,
                Username = _username,
                Password = new NetworkCredential("",_password).Password,
                Name = _name,
                LastName = _lastName,
                UserType = _userType
            };
            _userRepository.Add(_userModel);
            GoBackCommand.Execute(null);
            
            
        }

        //properties
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public string UserType
        {
            get => _userType;
            set
            {
                _userType = value;
                OnPropertyChanged(nameof(UserType));
            }
        }
        
    }
}
