using System;
using System.Threading;
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

        NavigateLendings = new GoLendingsCommand(null,
            new NavigationService<LendingsViewModel>(navigationStore, () => new LendingsViewModel(navigationStore)));
        NavigateBooks = new GoBooksCommand(null,
            new NavigationService<BooksViewModel>(navigationStore, () => new BooksViewModel(navigationStore)));
        NavigateUsers = new GoUsersCommand(null, new
            NavigationService<UsersViewModel>(navigationStore, () => new UsersViewModel(navigationStore, null)));
        NavigateMembers = new GoMembersCommand(null,
            new NavigationService<MembersViewModel>(navigationStore,
                () => new MembersViewModel(navigationStore)));

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

    #endregion


    #region ICommands

    public ICommand NavigateLendings { get; }
    public ICommand NavigateUsers { get; }
    public ICommand NavigateBooks { get; }
    public ICommand NavigateMembers { get; }

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

    #endregion
}