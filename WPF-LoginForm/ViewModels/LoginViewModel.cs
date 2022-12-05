using System.Net;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBiblioteca.Helpers;
using WPFBiblioteca.Repositories;

namespace WPFBiblioteca.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IUserRepository userRepository; // interface de usuario
    private string _dateTime;
    private string _email;
    private string _errorMessage;
    private bool _isViewVisible = true;
    private readonly MailService _mailService;

    private SecureString _password;

    //Fields
    private string _username;
    private string _usernameLog;
    private bool _visibility;

    //Constructor
    public LoginViewModel()
    {
        _visibility = false;
        _mailService = new MailService();
        var date = System.DateTime.Now;
        _dateTime = date.Date.ToShortDateString();
        userRepository = new UserRepository();
        LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPassCommand(_username, ""));
        AcceptEmailCommand = new ViewModelCommand(AcceptEmail);
        CancelEmailCommand = new ViewModelCommand(CancelEmail);
    }


    #region Properties

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

    public string UsernameLog
    {
        get => _usernameLog;
        set
        {
            if (value == _usernameLog) return;
            _usernameLog = value;
            OnPropertyChanged(nameof(UsernameLog));
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

    #endregion

    #region ICommands

    public ICommand LoginCommand { get; }
    public ICommand RecoverPasswordCommand { get; }
    public ICommand AcceptEmailCommand { get; }
    public ICommand CancelEmailCommand { get; }
    public ICommand ShowPasswordCommand { get; }
    public ICommand RememberPasswordCommand { get; }

    #endregion

    #region Methods

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
        Visibility = true;
    }

    private void CancelEmail(object obj)
    {
        Visibility = false;
    }

    private void AcceptEmail(object obj)
    {
        var user = userRepository.GetByUsername(_usernameLog);

        if (!Task.Run(() => userRepository.VerifyMail(_email)).Result) return;
        if (user == null || user.Email != _email) return;
        var generator = new RandomPasswordGenerator();
        var pass = generator.GeneratePassword(true, true, true, false, 6);
        user.Password = pass;
        userRepository.Edit(user, user.Id);
        var body =
            "<h1 style=\"text-align:center\"><span style=\"color:#00785c\"><strong><span style=\"font-size:48px\">Biblioteca ITSPP</span></strong></span></h1><p style=\"text-align:center\"><span style=\"color:#00785c\"><strong><span style=\"font-size:26px\">Datos de la cuenta</span></strong></span></p><hr /><p style=\"text-align:center\"><span style=\"font-size:28px\"><span style=\"color:#00785c\"><strong>Su nombre de usuario es:</strong></span></span></p><p style=\"text-align:center\"><u><span style=\"color:#00785c\"><span style=\"font-size:26px\">/?name/?</span></span></u></p><p style=\"text-align:center\"><span style=\"color:#00785c\"><strong><span style=\"font-size:26px\">Su nueva contrase&ntilde;a es:</span></strong></span></p><p style=\"text-align:center\"><u><span style=\"color:#00785c\"><span style=\"font-size:26px\">/?pass/?</span></span></u></p><hr /><p style=\"text-align:center\"><em><span style=\"color:#00785c\"><span style=\"font-size:26px\"><u>Porfavor cambie la contrase&ntilde;a a una que pueda recordar facilmente</u></span></span></em></p>"
                .Replace("/?pass/?", pass).Replace("/?name/?", user.Username);

        _mailService.Send(_email, "BibliotecaTecnm@outlook.com", @"F4tvFh~gJ9c!C>fx,1GRc2-e$h", body,
            "Recuperacion de su contraseña");
    }
}

#endregion