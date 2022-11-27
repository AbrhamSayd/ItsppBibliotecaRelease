using System.Windows.Input;
using MySqlConnector;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;

namespace WPFBiblioteca.ViewModels.Fields;

public class UserFieldsViewModel : ViewModelBase
{
    #region Methods

    private async void ExecuteEditionCommand(object obj)
    {
        if (_mode == "Add")
        {
            _userModel = new UserModel
            {
                Id = _id,
                Username = _username,
                Password = _password,
                FirstName = _firstName,
                LastName = _lastName,
                UserType = _userType
            };
            await _userRepository.Add(_userModel);
            GoBackCommand.Execute(null);
        }
        else
        {
            await _userRepository.Edit(_userModel, _staticId);
            GoBackCommand.Execute(null);
        }

        _errorCode = _userRepository.GetError();
    }

    #endregion

    #region Fields

    private UserModel _userModel;
    private int _id;
    private int _staticId;
    private string _username;
    private string _password;
    private string _firstName;
    private string _lastName;
    private string _userType;
    private readonly string _mode;
    private readonly IUserRepository _userRepository;
    private MySqlException _errorCode;

    #endregion

    #region Icommands

    public ICommand EditionCommand { get; }
    public ICommand GoBackCommand { get; }

    #endregion

    #region Constructor

    public UserFieldsViewModel(UserModel editUser, string mode, NavigationStore navigationStore)
    {
        _mode = mode;
        _userModel = editUser ?? new UserModel();
        _userRepository = new UserRepository();
        GoBackCommand = new GoUsersCommand(null,
            new NavigationService<UsersViewModel>(navigationStore,
                () => new UsersViewModel(navigationStore)));

        EditionCommand = new ViewModelCommand(ExecuteEditionCommand);


        if (mode == "Edit")
            FillModel();
    }


    private void FillModel()
    {
        _staticId = _userModel.Id;
        Id = _userModel.Id;
        Username = _userModel.Username;
        Password = _userModel.Password;
        FirstName = _userModel.FirstName;
        LastName = _userModel.LastName;
        UserType = _userModel.UserType;
    }

    #endregion

    #region Properties

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            _userModel.Id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            _userModel.Username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            _userModel.Password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            _userModel.FirstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            _userModel.LastName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }

    public string UserType
    {
        get => _userType;
        set
        {
            _userType = value;
            _userModel.UserType = value;
            OnPropertyChanged(nameof(UserType));
        }
    }

    #endregion
}