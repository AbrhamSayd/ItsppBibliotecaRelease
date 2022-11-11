using System.Windows.Input;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Stores;


namespace WPFBiblioteca.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        //fields
        private readonly IUserRepository _userRepository;
        private UserModel _userModel;

        private string _id;
        private string _username;
        private string _password;
        private string _name;
        private string _lastName;
        private string _userType;

        //Icommands
        public ICommand AddCommand { get; }
        //constructor
        public UsersViewModel(NavigationStore navigationStore)
        {
            _userRepository = new UserRepository();
            AddCommand = new ViewModelCommand(ExecuteAddCommand);
        }
        //methods
        private void ExecuteAddCommand(object obj)
        {
            var user = new UserModel
            {
                Id = _id,
                Username = _username,
                Password = _password,
                Name = _name,
                LastName = _lastName,
                UserType = _userType
            };
            _userRepository.Add(user);
        }

        //properties
        public string Id
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
        public string Password
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
