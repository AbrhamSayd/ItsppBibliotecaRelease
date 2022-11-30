using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Security.RightsManagement;
using System.Windows;
using System.Windows.Input;
using MySqlConnector;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using static System.Reflection.Metadata.BlobBuilder;

namespace WPFBiblioteca.ViewModels.Fields;

public class UserFieldsViewModel : ViewModelBase
{
    #region Fields

    private UserModel _userModel;
    private int _id;
    private int _staticId;
    private string _username;
    private string _password;
    private string _firstName;
    private string _lastName;
    private string _userType;
    private string _element;
    private string _title;
    private bool _visibility;
    private readonly string _mode;
    private readonly IUserRepository _userRepository;
    private ObservableCollection<UserModel> _users;
    private string _errorFocus;
    private string _errorCode;

    #endregion

    #region Icommands

    public ICommand EditionCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand AcceptCommand { get; }

    #endregion

    #region Methods

    private async void ExecuteEditionCommand(object obj)
    {
        var isDuplicate = false;
        if (_mode == "Add")
        {
            foreach (var user in Users)
            {
                if (user.Id == _id)
                {
                    Element = "Numero de empleado duplicado, Intente con otro porfavor";
                    Title = "Dato duplicado";
                    Visibility = true;
                    isDuplicate = true;
                    _errorFocus = "Id";
                }
                else if (_username == user.Username)
                {
                    Element = "Nombre de usuario duplicado, Intente con otro porfavor";
                    Title = "Dato duplicado";
                    Visibility = true;
                    isDuplicate = true;
                    _errorFocus = "Username";
                }
            }

            if (isDuplicate) return;
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
            _errorCode = _userRepository.GetError();
        }
    }

    private async void ExecuteGetAllCommand(object o)
    {
        Users = new ObservableCollection<UserModel>(await _userRepository.GetByAll());
        _errorCode = _userRepository.GetError();
    }

    private void ExecuteAcceptCommand(object obj)
    {
        switch (_errorFocus)
        {
            case "Id":
                Id = 0;
                break;
            case "Username":
                Username = string.Empty;
                break;
        }

        Visibility = false;
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

    public string Title
    {
        get => _title;
        set
        {
            if (value == _title) return;
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    public string Element
    {
        get => _element;
        set
        {
            if (value == _element) return;
            _element = value;
            OnPropertyChanged(nameof(Element));
        }
    }

    public bool Visibility
    {
        get => _visibility;
        set
        {
            if (value == _visibility) return;
            _visibility = value;
            OnPropertyChanged(nameof(Visibility));
        }
    }

    public ObservableCollection<UserModel> Users
    {
        get => _users;
        set
        {
            if (Equals(value, _users)) return;
            _users = value;
            OnPropertyChanged(nameof(Users));
        }
    }

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
        AcceptCommand = new ViewModelCommand(ExecuteAcceptCommand);
        ExecuteGetAllCommand(null);
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
}