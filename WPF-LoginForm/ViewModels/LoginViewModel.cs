using System;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Windows.Input;
using WPFBiblioteca.Repositories;

namespace WPFBiblioteca.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private string _errorMessage;
    private bool _isViewVisible = true;

    private SecureString _password;

    //Fields
    private string _username;

    private readonly IUserRepository userRepository; // interface de usuario

    //Constructor
    public LoginViewModel()
    {
        userRepository = new UserRepository();
        LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPassCommand("", ""));
    }

    //Properties
    public string Username
    {
        get => _username;

        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username)); //Mandamos a informar que la propiedad cambio
        }
    }

    public SecureString Password
    {
        get => _password;

        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;

        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }

    public bool IsViewVisible
    {
        get => _isViewVisible;

        set
        {
            _isViewVisible = value;
            OnPropertyChanged(nameof(IsViewVisible));
        }
    }

    //-> Commands
    public ICommand LoginCommand { get; }
    public ICommand RecoverPasswordCommand { get; }
    public ICommand ShowPasswordCommand { get; }
    public ICommand RememberPasswordCommand { get; }

    private bool CanExecuteLoginCommand(object obj)
    {
        //verificamos si los datos son correctos para avisar que si podemos ejecutar
        bool validData;
        if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
            Password == null || Password.Length < 3)
            validData = false;
        else
            validData = true;
        return validData;
    }

    private async void ExecuteLoginCommand(object obj)
    {
        var isValidUser = await userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
        if (isValidUser)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(
                new GenericIdentity(Username), null);
            IsViewVisible = false;
        }
        else
        {
            ErrorMessage = "* Contraseña o nombre de usuario invalido";
        }
    }

    private void ExecuteRecoverPassCommand(string username, string email)
    {
        throw new NotImplementedException();
    }
}