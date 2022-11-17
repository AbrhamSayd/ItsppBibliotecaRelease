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
        private readonly IUserRepository _userRepository;
        private int _id;
        private string _username;
        private string _password;
        private string _name;
        private string _lastName;
        private string _userType;

        public ViewModelBase CurrentViewModel=> _navigationStore.CurrentViewModel;

        
        
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

        
        
        public MainViewModel(NavigationStore navigationStore)
        {

            _navigationStore = navigationStore ?? throw new ArgumentNullException(nameof(navigationStore));
           // _navigationStore.CurrentViewModel 
            _userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
            
            NavigateBooks = new GoBooksCommand(null,
                new NavigationService<BooksViewModel>(navigationStore, (() => new BooksViewModel(navigationStore))));
            NavigateUsers = new GoUsersCommand(null,new 
                NavigationService<UsersViewModel>(navigationStore, () => new UsersViewModel(navigationStore)));

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

        }


        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        
        public ICommand NavigateUsers{ get; }
        public ICommand NavigateBooks { get; }


        private void LoadCurrentUserData()
        {
            var user = _userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
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
