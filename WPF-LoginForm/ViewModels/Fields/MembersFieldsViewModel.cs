﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;

namespace WPFBiblioteca.ViewModels.Fields;

internal static class Email
{
    internal static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }
}
internal static class TryConvert
{
    /// <summary>
    /// Returns the integer result of parsing a string, or null.
    /// </summary>
    
    internal static int? ToNullableInt32(string toParse)
    {
        int result;
        if (Int32.TryParse(toParse, out result)) return result;
        return null;
    }

    internal static long? ToNullableLong(string toParse)
    {
        long result;
        if (long.TryParse(toParse, out result)) return result;
        return null;
        
            
        
    }

    /// <summary>
    /// Returns the integer result of parsing a string,
    /// or the supplied failure value if the parse fails.
    /// </summary>
    internal static int ToInt32(string toParse, int toReturnOnFailure)
    {
        // The nullable-result method sets up for a coalesce operator.
        return ToNullableInt32(toParse) ?? toReturnOnFailure;
    }

    internal static long ToLong(string toParse, int toReturnOnFailure)
    {
        return  ToNullableLong(toParse) ?? toReturnOnFailure;
    }
}

public class MembersFieldsViewModel : ViewModelBase
{
    #region Fields

    private MemberModel _member;
    private string _memberId;
    private string _firstName;
    private string _lastName;
    private string _carrera;
    private string _email;
    private string _phoneNumber;
    private bool _deudor;
    private string _prestamos;
    private int _staticId;
    private string _element;
    private string _title;
    private bool _visibility;
    private string _errorCode;
    private string _errorFocus;
    private readonly string _mode;
    private ObservableCollection<MemberModel> _members;
    private readonly IMemberRepository _membersRepository;

    #endregion

    #region ICommands

    public ICommand EditionCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand AcceptCommand { get; }

    #endregion

    #region Methods

    private void FillModel()
    {
        _staticId = _member.MemberId;
        _memberId = _member.MemberId.ToString();
        _firstName = _member.FirstName;
        _lastName = _member.LastName;
        _carrera = _member.Carrera;
        _email = _member.Email;
        _phoneNumber = _member.PhoneNumber;
        _deudor = _member.Deudor;
        _prestamos = _member.Prestamos.ToString();
    }

    private bool CanExecuteEdition(object obj)
    {
        return TryConvert.ToInt32(MemberId, 0) != 0 && MemberId.Length > 3 && !string.IsNullOrEmpty(MemberId) &&
               FirstName.Length > 3 && !string.IsNullOrEmpty(FirstName) && LastName.Length > 4 && !string.IsNullOrEmpty(LastName) && Fields.Email.IsValidEmail(Email);
    }

    private async void ExecuteEditionCommand(object obj)
    {
        var isDuplicate = false;
        if (_mode == "Add")
        {
            foreach (var member in Members)
            {
                if (_memberId != member.MemberId.ToString()) continue;
                Element = "Id duplicada, Intente con otra porfavor";
                Title = "Dato duplicado";
                Visibility = true;
                isDuplicate = true;
            }

            if (isDuplicate) return;
            _member = new MemberModel
            {
                MemberId = TryConvert.ToInt32(_memberId, 0),
                FirstName = _firstName,
                LastName = _lastName,
                Carrera = _carrera,
                Email = _email,
                PhoneNumber = _phoneNumber,
                Deudor = _deudor,
                Prestamos = TryConvert.ToInt32(_prestamos, 0)
            };
            await _membersRepository.Add(_member);
            GoBackCommand.Execute(null);
        }
        else
        {
            _member = new MemberModel
            {
                MemberId = TryConvert.ToInt32(_memberId, 0),
                FirstName = _firstName,
                LastName = _lastName,
                Carrera = _carrera,
                Email = _email,
                PhoneNumber = _phoneNumber,
                Deudor = _deudor,
                Prestamos = TryConvert.ToInt32(_prestamos, 0)
            };
            await _membersRepository.Add(_member);
            GoBackCommand.Execute(null);
        }
    }

    private async void ExecuteGetAllCommand(object o)
    {
        Members = new ObservableCollection<MemberModel>(await _membersRepository.GetByAll());

        _errorCode = _membersRepository.GetError();
    }

    private void ExecuteAcceptCommand(object obj)
    {
        switch (_errorFocus)
        {
            case "Id":
                MemberId = string.Empty;
                break;
        }

        Visibility = false;
    }
    #endregion

    #region Properties
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
    public string MemberId
    {
        get => _memberId;
        set
        {
            if (value == _memberId) return;
            _memberId = value;
            _member.MemberId = TryConvert.ToInt32(value, 0);
            OnPropertyChanged(nameof(MemberId));
        }
    }

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (value == _firstName) return;
            _firstName = value;
            _member.FirstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (value == _lastName) return;
            _lastName = value;
            _member.LastName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }

    public string Carrera
    {
        get => _carrera;
        set
        {
            if (value == _carrera) return;
            _carrera = value;
            _member.Carrera = value;
            OnPropertyChanged(nameof(Carrera));
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (value == _email) return;
            _email = value;
            _member.Email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (value == _phoneNumber) return;
            _phoneNumber = value;
            _member.PhoneNumber = value;
            OnPropertyChanged(nameof(PhoneNumber));
        }
    }

    public bool Deudor
    {
        get => _deudor;
        set
        {
            if (value == _deudor) return;
            _deudor = value;
            _member.Deudor = value;
            OnPropertyChanged(nameof(Deudor));
        }
    }

    public string Prestamos
    {
        get => _prestamos;
        set
        {
            if (value == _prestamos) return;
            _prestamos = value;
            _member.Prestamos = TryConvert.ToInt32(_prestamos, 0);
            OnPropertyChanged(nameof(Prestamos));
        }
    }

    public int StaticId
    {
        get => _staticId;
        set
        {
            if (value == _staticId) return;
            _staticId = value;
            OnPropertyChanged(nameof(StaticId));
        }
    }

    public ObservableCollection<MemberModel> Members
    {
        get => _members;
        set
        {
            if (Equals(value, _members)) return;
            _members = value;
            OnPropertyChanged(nameof(Members));
        }
    }

    #endregion

    #region Constructor

    public MembersFieldsViewModel(MemberModel member, string mode, NavigationStore navigationStore)
    {
        _email = "";
        _mode = mode;
        _member = member ?? new MemberModel();

        GoBackCommand = new GoMembersCommand(null,
            new NavigationService<MembersViewModel>(navigationStore, () => new MembersViewModel(navigationStore)));
        _membersRepository = new MemberRepository();
        EditionCommand = new ViewModelCommand(ExecuteEditionCommand,CanExecuteEdition);
        AcceptCommand = new ViewModelCommand(ExecuteAcceptCommand);
        if (mode == "Edit") FillModel();
        ExecuteGetAllCommand(null);
    }

    #endregion
}