using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Helpers;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;

namespace WPFBiblioteca.ViewModels.Fields;

public class UserFieldsViewModel : ViewModelBase
{
    #region Fields

    private UserModel _userModel;
    private string _id;
    private int _staticId;
    private int _staticEditId;
    private string _staticEditUsern;
    private string _username;
    private string _password;
    private string _firstName;
    private string _lastName;
    private string _userType;
    private string _element;
    private string _title;
    private bool _visibility;
    private string _email;
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
        var staticId = _staticId;
        var staticUserName = _staticEditUsern;
        var isDuplicate = false;
        foreach (var user in Users)
        {
            if (user.Id == staticId || user.Username == staticUserName) continue;
            if (user.Id.ToString() == _id)
            {
                Element = "Número de empleado duplicado, Intente con otro por favor";
                Title = "Dato duplicado";
                Visibility = true;
                isDuplicate = true;
                _errorFocus = "Id";
                break;
            }

            if (_username != user.Username) continue;
            Element = "Nombre de usuario duplicado, Intente con otro por favor";
            Title = "Dato duplicado";
            _visibility = true;
            isDuplicate = true;
            _errorFocus = "Username";
            break;
        }

        if (isDuplicate) return;

        switch (_mode)
        {
            case "Add":
                _userModel = new UserModel
                {
                    Id = ValidationHelper.TryConvert.ToInt32(_id, 0),
                    Username = _username,
                    Password = _password,
                    FirstName = _firstName,
                    LastName = _lastName,
                    UserType = _userType,
                    Email = _email
                };
                await _userRepository.Add(_userModel);
                GoBackCommand.Execute(null);
                break;
            case "Edit":
                _userModel = new UserModel
                {
                    Id = ValidationHelper.TryConvert.ToInt32(_id, 0),
                    Username = _username,
                    Password = _password,
                    FirstName = _firstName,
                    LastName = _lastName,
                    UserType = _userType,
                    Email = _email
                };
                await _userRepository.Edit(_userModel, _staticId);
                GoBackCommand.Execute(null);
                _errorCode = _userRepository.GetError();
                break;
        }
    }

    private bool CanExecuteEdition(object obj)
    {
        return Id != "0" && Username is { Length: > 5 } && Password is { Length: > 4 } &&
               FirstName is { Length: > 3 } && UserType != null && !string.IsNullOrEmpty(Email) &&
               ValidationHelper.Email.IsValidEmail(Email);
    }

    private async void ExecuteGetAllCommand()
    {
        Users = new ObservableCollection<UserModel>(await _userRepository.GetByAll());
        _errorCode = _userRepository.GetError();
    }

    private void ExecuteAcceptCommand(object obj)
    {
        switch (_errorFocus)
        {
            case "Id":
                Id = string.Empty;
                break;
            case "Username":
                Username = string.Empty;
                break;
        }

        Visibility = false;
    }

    #endregion

    #region Properties

    public string Id
    {
        get => _id;
        set
        {
            _id = value;
            _userModel.Id = ValidationHelper.TryConvert.ToInt32(value, 0);
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

    public string Email
    {
        get => _email;
        set
        {
            if (value == _email) return;
            _email = value;
            OnPropertyChanged(nameof(Email));
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
        _userType = "admin";
        _userModel = editUser ?? new UserModel();
        _userRepository = new UserRepository();
        GoBackCommand = new GoUsersCommand(null,
            new NavigationService<UsersViewModel>(navigationStore,
                () => new UsersViewModel(navigationStore)));

        EditionCommand = new ViewModelCommand(ExecuteEditionCommand, CanExecuteEdition);
        AcceptCommand = new ViewModelCommand(ExecuteAcceptCommand);
        ExecuteGetAllCommand();
        if (mode == "Edit") FillModel();
    }


    private void FillModel()
    {
        _staticId = _userModel.Id;
        Id = _userModel.Id.ToString();
        Username = _userModel.Username;
        Password = _userModel.Password;
        FirstName = _userModel.FirstName;
        LastName = _userModel.LastName;
        UserType = "admin";
        Email = _userModel.Email;
        _staticEditId = ValidationHelper.TryConvert.ToInt32(Id, 0);
        _staticEditUsern = Username;
    }

    #endregion
}