using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Windows.Networking;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Stores;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WPFBiblioteca.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //Fields
        private readonly NavigationStore _navigationStore;
        private UserAccountModel _currentUserAccount;
        private IUserRepository userRepository;
        private UserModel _userModel;
        private string _id;
        private string _username;
        private string _password;
        private string _name;
        private string _lastName;
        private string _userType;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        
        
        public string id
        {
            get => _id;
            set 
            {
                _id = value;
                OnPropertyChanged(nameof(id));
            }
        }
        public string username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(username));
            }
        }
        public string password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(password));
            }
        }
        public string name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(name));
            }
        }

        public string lastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(lastName));
            }
        }
        public string userType
        {
            get => _userType;
            set
            {
                _userType = value;
                OnPropertyChanged(nameof(userType));
            }
        }
        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        
        
        public MainViewModel()
        {
            _navigationStore = new NavigationStore();
            if (_navigationStore.CurrentViewModel != null) _navigationStore.CurrentViewModel = new MainViewModel();
            NavigateUsersCommand = new ViewModelCommand(ExecuteNavigateUsersCommand);
            AddCommand = new ViewModelCommand(ExecuteAddCommand);
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;


        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void ExecuteNavigateUsersCommand(object obj)
        {
            _navigationStore.CurrentViewModel = new UsersViewModel();
        }


        private void ExecuteAddCommand(object obj)
        {
            var a = new UserModel
            {
                Id = _id,
               Username = _username,
               Password = _password,
               Name = _name,
               LastName = _lastName,
               UserType = _userType
            };
            userRepository.Add(a);
        }

        public ICommand AddCommand { get; }
        public ICommand NavigateUsersCommand { get; }
        

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"Bienvenido {user.Name} {user.LastName} ;)";
                CurrentUserAccount.ProfilePicture = null;               
            }
            else
            {
                CurrentUserAccount.DisplayName="Usuario invalido, no loggeado";
                //Hide child views.
            }
        }
        
        
    }
}
