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
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
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
        private int _id;
        private string _username;
        private string _password;
        private string _name;
        private string _lastName;
        private string _userType;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        
        
        public int id
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
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
            NavigateUsersCommand = new NavigateCommand<UsersViewModel>(new NavigationService<UsersViewModel>(_navigationStore, () => new UsersViewModel(_navigationStore)));
            NavigateBooksCommand = new NavigateCommand<BooksViewModel>(new NavigationService<BooksViewModel>(_navigationStore, () => new BooksViewModel(_navigationStore)));

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

        }


        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        
        public ICommand NavigateUsersCommand { get; }
        public ICommand NavigateBooksCommand { get; }


        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"Bienvenido {user.FirstName} {user.LastName} ;)";
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
