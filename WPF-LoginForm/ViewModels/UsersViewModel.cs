using System.Collections.ObjectModel;
using System.Windows.Input;
using MySqlConnector;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels.Fields;

namespace WPFBiblioteca.ViewModels;

public class UsersViewModel : ViewModelBase
{
    #region constructor

    public UsersViewModel(NavigationStore navigationStore, MySqlException errorCode)
    {
        _errorCode = errorCode;
        _canDelete = false;
        _userRepository = new UserRepository();
        _usersModelRow = new UserModel();
        NavigateAddCommand = new NavigateCommand<UserFieldsViewModel>(
            new NavigationService<UserFieldsViewModel>(navigationStore,
                () => new UserFieldsViewModel(null, "Add", navigationStore)));


        ExecuteGetAllCommand(null);
        _errorCode = _userRepository.GetError();
        GetByAllCommand = new ViewModelCommand(ExecuteGetAllCommand);
        EditRowCommand = new NavigateCommand<UserFieldsViewModel>(
            new NavigationService<UserFieldsViewModel>(navigationStore,
                () => new UserFieldsViewModel(_usersModelRow, "Edit", navigationStore)));
        RemoveRowCommand = new ViewModelCommand(ExecuteRemoveRowCommand, CanExecuteRemoveRowCommand);
    }

    #endregion

    #region Fields

    private ObservableCollection<UserModel> _collectionUsers;
    private UserModel _usersModelRow;
    private readonly IUserRepository _userRepository;
    private bool _canDelete;
    private MySqlException _errorCode;
    private string _errorMessage;

    #endregion

    #region ICommands

    public ICommand GetByAllCommand { get; }
    public ICommand NavigateAddCommand { get; }
    public ICommand RemoveRowCommand { get; }
    public ICommand EditRowCommand { get; }

    #endregion


    #region Methods

    private bool CanExecuteRemoveRowCommand(object obj)
    {
        _errorCode = _userRepository.GetError();
        return _canDelete;
    }

    private void ExecuteRemoveRowCommand(object obj)
    {
        _userRepository.Remove(_usersModelRow.Id);
        ExecuteGetAllCommand(null);
    }

    private async void ExecuteGetAllCommand(object obj)
    {
        CollectionUsers = new ObservableCollection<UserModel>(await _userRepository.GetByAll());
    }

    #endregion

    #region Properties

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }

    public MySqlException ErrorCode
    {
        get => _errorCode;
        set
        {
            _errorCode = value;
            if (_errorCode.SqlState != null) ErrorMessage = _errorCode.SqlState;
            OnPropertyChanged(nameof(ErrorCode));
        }
    }

    public bool CanDelete
    {
        get => _canDelete;
        set
        {
            _canDelete = value;
            OnPropertyChanged(nameof(CanDelete));
        }
    }

    public ObservableCollection<UserModel> CollectionUsers
    {
        get => _collectionUsers;
        set
        {
            _collectionUsers = value;
            OnPropertyChanged(nameof(CollectionUsers));
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