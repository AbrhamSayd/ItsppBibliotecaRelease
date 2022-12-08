using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels.Fields;

namespace WPFBiblioteca.ViewModels;

public class MembersViewModel : ViewModelBase
{
    #region Constructor

    public MembersViewModel(NavigationStore navigationStore)
    {
        _canDelete = false;
        _errorCode = string.Empty;
        _navigationStore = navigationStore;
        _membersRepository = new MemberRepository();
        _member = new MemberModel();
        _collectionMembers = new ObservableCollection<MemberModel>();
        _title = "miembros";
        _element = null;
        _visibility = false;
        AddCommand = new NavigateCommand<MembersFieldsViewModel>(
            new NavigationService<MembersFieldsViewModel>(navigationStore,
                () => new MembersFieldsViewModel(null, "Add", navigationStore)));
        EditCommand = new NavigateCommand<MembersFieldsViewModel>(
            new NavigationService<MembersFieldsViewModel>(navigationStore,
                () => new MembersFieldsViewModel(_member, "Edit", navigationStore)));
        RemoveCommand = new ViewModelCommand(ExecuteRemoveCommand, CanExecuteRemove);
        AcceptRemoveCommand = new ViewModelCommand(ExecuteRemove);
        CancelRemoveCommand = new ViewModelCommand(CancelRemove);
        ExecuteGetAllCommand();
    }

    #endregion

    #region Fields

    private ObservableCollection<MemberModel> _collectionMembers;
    private MemberModel _member;
    private readonly IMemberRepository _membersRepository;
    private NavigationStore _navigationStore;
    private string _errorCode;
    private bool _canDelete;
    private string _title;
    private string _element;
    private bool _visibility;

    #endregion

    #region ICommands

    public ICommand AddCommand { get; }
    public ICommand RemoveCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand AcceptRemoveCommand { get; }
    public ICommand CancelRemoveCommand { get; }

    #endregion

    #region Methods

    private bool CanExecuteRemove(object obj)
    {
        return _member != null;
    }

    private async void ExecuteGetAllCommand()
    {
        CollectionMembers = new ObservableCollection<MemberModel>(await _membersRepository.GetByAll());

        _errorCode = _membersRepository.GetError();
    }

    private void ExecuteRemoveCommand(object obj)
    {
        Element = _member.FirstName + " " + _member.LastName;
        Visibility = true;
    }

    private void ExecuteRemove(object obj)
    {
        Visibility = false;
        _membersRepository.Delete(_member.MemberId);
        CollectionMembers.Remove(_member);
    }

    private void CancelRemove(object obj)
    {
        Visibility = false;
    }

    #endregion

    #region Properties

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
            if (value == _canDelete) return;
            _canDelete = value;
            OnPropertyChanged(nameof(CanDelete));
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

    public MemberModel Member
    {
        get => _member;
        set
        {
            if (Equals(value, _member)) return;
            _member = value;
            CanDelete = _member != null;
            OnPropertyChanged(nameof(Member));
        }
    }

    public ObservableCollection<MemberModel> CollectionMembers
    {
        get => _collectionMembers;
        set
        {
            _collectionMembers = value;
            OnPropertyChanged(nameof(CollectionMembers));
        }
    }

    #endregion
}