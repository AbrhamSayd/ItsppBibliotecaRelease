using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;

namespace WPFBiblioteca.ViewModels;

public class MainViewModel : ViewModelBase
{
    #region Constructor

    public MainViewModel(NavigationStore navigationStore)
    {
        
        _navigationStore = navigationStore ?? throw new ArgumentNullException(nameof(navigationStore));
        // _navigationStore.CurrentViewModel 
        _userRepository = new UserRepository();
        CurrentUserAccount = new UserAccountModel();
        LoadCurrentUserData();
        ShowCurrentUserMenu = new ViewModelCommand(ShowCurrentUser);
        _isEnabledButton = true;
        NavigateHome = new GoHomeCommand(null,
            new NavigationService<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore,_currentUser)));
        NavigateLendings = new GoLendingsCommand(null,
            new NavigationService<LendingsViewModel>(navigationStore,
                () => new LendingsViewModel(navigationStore, _currentUser)));
        NavigateBooks = new GoBooksCommand(null,
            new NavigationService<BooksViewModel>(navigationStore, () => new BooksViewModel(navigationStore)));
        NavigateUsers = new GoUsersCommand(null,
            new NavigationService<UsersViewModel>(navigationStore, () => new UsersViewModel(navigationStore)));
        NavigateMembers = new GoMembersCommand(null,
            new NavigationService<MembersViewModel>(navigationStore, () => new MembersViewModel(navigationStore)));

        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    #endregion

    #region Fields

    
    private readonly NavigationStore _navigationStore;
    private UserAccountModel _currentUserAccount;
    private readonly IUserRepository _userRepository;
    private int _id;
    private string _username;
    private string _password;
    private string _name;
    private string _lastName;
    private string _userType;
    private UserModel _currentUser;
    private bool _visibility;
    private bool _isEnabledButton;
    private int _blurRadius;
    private string _dateTime;


    #endregion

    #region ICommands

    public ICommand NavigateLendings { get; }
    public ICommand NavigateUsers { get; }
    public ICommand NavigateBooks { get; }
    public ICommand NavigateMembers { get; }
    public ICommand NavigateHome { get; }
    public ICommand ShowCurrentUserMenu { get; }
    public ICommand Close { get; }

    #endregion

    #region Methods

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    private void LoadCurrentUserData()
    {
        if (Thread.CurrentPrincipal != null)
        {
            var user = _userRepository.GetByUsername(Thread.CurrentPrincipal.Identity!.Name);
            if (user != null)
            {
                _name = user.FirstName + " " + user.LastName;
                _currentUser = user;
                _username = user.Username;
                _id = user.Id;
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"Bienvenido {user.FirstName} {user.LastName} ;)";
                CurrentUserAccount.ProfilePicture = null;
            }
            else
            {
                CurrentUserAccount.DisplayName = "Usuario invalido, no loggeado";
                //Hide child views.
            }
        }
    }

    private void ShowCurrentUser(object obj)
    {
        if (Visibility == true )
        {
            IsEnabledButton = true;
            Visibility = false;
            BlurRadius = 0;
        }
        else
        {
            IsEnabledButton = false;
            Visibility = true;
            BlurRadius = 12;
        }
    }

    public void HideCUpopup(object obj)
    {
        IsEnabledButton = true;
        Visibility = false;
        BlurRadius = 0;
    }


    #endregion

    #region Properties

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

    public string UserType
    {
        get => _userType;
        set
        {
            _userType = value;
            OnPropertyChanged(nameof(UserType));
        }
    }

    public UserAccountModel CurrentUserAccount
    {
        get => _currentUserAccount;

        set
        {
            _currentUserAccount = value;
            OnPropertyChanged(nameof(CurrentUserAccount));
        }
    }

    public int BlurRadius
    {
        get => _blurRadius;
        set
        {
            _blurRadius = value;
            OnPropertyChanged(nameof(BlurRadius));
        }
    }

    public bool Visibility
    {
        get => _visibility;
        set
        {
            _visibility = value;
            OnPropertyChanged(nameof(Visibility));
        }
    }

    public bool IsEnabledButton
    {
        get => _isEnabledButton;
        set
        {
            _isEnabledButton = value;
            OnPropertyChanged(nameof(IsEnabledButton));
        }
    }

    public string DateTime
    {
        get => _dateTime;
        set
        {
            if (value == _dateTime) return;
            _dateTime = value;
            OnPropertyChanged(nameof(DateTime));
        }
    }

    #endregion
}

