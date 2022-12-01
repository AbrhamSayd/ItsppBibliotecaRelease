using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels.Fields;

namespace WPFBiblioteca.ViewModels;

public class UsersViewModel : ViewModelBase
{
   

    #region Fields

    private ObservableCollection<UserModel> _collectionUsers;
    private UserModel _usersModel;
    private readonly IUserRepository _userRepository;
    private bool _canDelete;
    private string _errorCode;
    private string _errorMessage;
    private readonly NavigationStore _navigationStore;
    private string _title;
    private bool _removeVisibility;
    private bool _editVisibility;
    private string _element;
    private string _password;

    

        #endregion

        #region ICommands

    public ICommand NavigateAddCommand { get; }

    public ICommand RemoveCommand { get; }
    public ICommand AcceptRemoveCommand { get; }
    public ICommand CancelRemoveCommand { get; }

    public ICommand AcceptPasswordCommand { get; }
    public ICommand CancelPasswordCommand { get; }

    public ICommand NavigateEditCommand { get; } // No ejecutar directamente
    public ICommand EditCommand { get; } // No ejecutar directamente
    

    #endregion

    #region Methods

    private bool CanExecuteRemoveRowCommand(object obj)
    {
        return _usersModel != null;
    }

    private void ExecuteRemoveRowCommand(object obj)
    {
        Element =  _usersModel.FirstName + " " + _usersModel.LastName ;
        RemoveVisibility = true;
    }

    private async void ExecuteGetAllCommand(object obj)
    {
        CollectionUsers = new ObservableCollection<UserModel>(await _userRepository.GetByAll());
    }

    private void ExecuteRemove(object obj)
    {
        RemoveVisibility = false;
        _userRepository.Delete(_usersModel.Id);
        CollectionUsers.Remove(_usersModel);
    }

    private void CancelRemove(object obj)
    {
        RemoveVisibility = false;
        _password = string.Empty;
    }

    private void ExecuteEdit(object obj)
    {
        Element = "Introduzca la contraseña del usuario";
        Title = "Editar usuario";
        EditVisibility = true;
    }

    private void ValidateEdit(object obj)
    {
        if (_password == _usersModel.Password)
        {
            NavigateEditCommand.Execute(obj);
            EditVisibility = false;
        }
        else
        {
            ErrorMessage = "Contraseña incorrecta";
            EditVisibility = false;
        }
    }

    private void CancelEdit(object obj)
    {
        EditVisibility = false;
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

    public string ErrorCode
    {
        get => _errorCode;
        set
        {
            _errorCode = value;
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
        get => _usersModel;
        set
        {
            _usersModel = value;
            CanDelete = _usersModel != null;

            OnPropertyChanged(nameof(_usersModel));
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

    public bool RemoveVisibility
    {
        get => _removeVisibility;
        set
        {
            if (value == _removeVisibility) return;
            _removeVisibility = value;
            OnPropertyChanged(nameof(RemoveVisibility));
        }
    }
    public bool EditVisibility
    {
        get => _editVisibility;
        set
        {
            if (value == _editVisibility) return;
            _editVisibility = value;
            OnPropertyChanged(nameof(EditVisibility));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (value == _password) return;
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    #endregion

    #region constructor

    public UsersViewModel(NavigationStore navigationStore)
    {
        
        _canDelete = false;
        _errorCode = string.Empty;
        _userRepository = new UserRepository();
        _navigationStore = navigationStore;
        _usersModel = new UserModel();
        _title = "Usuarios";
        _element = null;
        _removeVisibility = false;
        _editVisibility = false;
        NavigateAddCommand = new NavigateCommand<UserFieldsViewModel>(
            new NavigationService<UserFieldsViewModel>(navigationStore,
                () => new UserFieldsViewModel(null, "Add", navigationStore)));
        NavigateEditCommand = new NavigateCommand<UserFieldsViewModel>(
            new NavigationService<UserFieldsViewModel>(navigationStore,
                () => new UserFieldsViewModel(_usersModel, "Edit", navigationStore)));
        RemoveCommand = new ViewModelCommand(ExecuteRemoveRowCommand, CanExecuteRemoveRowCommand);

        AcceptRemoveCommand = new ViewModelCommand(ExecuteRemove);
        CancelRemoveCommand = new ViewModelCommand(CancelRemove);

        EditCommand = new ViewModelCommand(ExecuteEdit);
        AcceptPasswordCommand = new ViewModelCommand(ValidateEdit);
        CancelPasswordCommand = new ViewModelCommand(CancelEdit);

        ExecuteGetAllCommand(null);
    }

    #endregion
}