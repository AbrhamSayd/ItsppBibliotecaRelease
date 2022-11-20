using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;

namespace WPFBiblioteca.ViewModels.Fields;

public class MembersFieldsViewModel : ViewModelBase
{
    #region Constructor

    public MembersFieldsViewModel(MemberModel member, string mode, NavigationStore navigationStore)
    {
        _mode = mode;
        _member = member ?? new MemberModel();

        GoBackCommand = new GoMembersCommand(null,
            new NavigationService<MembersViewModel>(navigationStore, () => new MembersViewModel(navigationStore)));
        _memberRepository = new MemberRepository();
        EditionCommand = new ViewModelCommand(ExecuteEditionCommand);
        if (mode == "Edit") FillModel();
    }

    #endregion

    #region Fields

    private MemberModel _member;
    private int _memberId;
    private string _firstName;
    private string _lastName;
    private string _carrera;
    private string _email;
    private string _phoneNumber;
    private bool _deudor;
    private int _prestamos;
    private int _staticId;
    private readonly string _mode;
    private readonly IMemberRepository _memberRepository;

    #endregion

    #region ICommands

    public ICommand EditionCommand { get; }
    public ICommand GoBackCommand { get; }

    #endregion

    #region Methods

    private void FillModel()
    {
        _staticId = _member.MemberId;
        _memberId = _member.MemberId;
        _firstName = _member.FirstName;
        _lastName = _member.LastName;
        _carrera = _member.Carrera;
        _email = _member.Email;
        _phoneNumber = _member.PhoneNumber;
        _deudor = _member.Deudor;
        _prestamos = _member.Prestamos;
    }

    private async void ExecuteEditionCommand(object obj)
    {
        if (_mode == "Add")
        {
            _member = new MemberModel
            {
                MemberId = _memberId,
                FirstName = _firstName,
                LastName = _lastName,
                Carrera = _carrera,
                Email = _email,
                PhoneNumber = _phoneNumber,
                Deudor = _deudor,
                Prestamos = _prestamos
            };


            await _memberRepository.Add(_member);
            GoBackCommand.Execute(null);
        }
        else
        {
            await _memberRepository.Edit(_member, _staticId);
            GoBackCommand.Execute(null);
        }
    }

    #endregion

    #region Properties

    public int MemberId
    {
        get => _memberId;
        set
        {
            if (value == _memberId) return;
            _memberId = value;
            _member.MemberId = value;
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

    public int Prestamos
    {
        get => _prestamos;
        set
        {
            if (value == _prestamos) return;
            _prestamos = value;
            _member.Prestamos = value;
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

    #endregion
}