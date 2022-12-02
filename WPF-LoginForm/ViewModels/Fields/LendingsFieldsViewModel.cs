using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Models.ComboBoxModels;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;

namespace WPFBiblioteca.ViewModels.Fields;

public class LendingsFieldsViewModel : ViewModelBase
{
    #region Fields

    
    private readonly string _mode;
    private LendingModel _lending;
    private readonly UserModel _currentUser;

    private readonly ILendingRepository _lendingRepository;
    private readonly IBookRepository _bookRepository;
    private ObservableCollection<CategoryModel> _lendings;


    private int _lendingId;
    private int _bookId;
    private string _isbn;
    private string _bookName;
    private int _memberId;
    private string _memberName;
    private DateTime _dateTimeBorrowed;
    private string _usernameLent;
    private DateTime _dateTimeReturned;
    private string _usernameReturned;
    private int _finedAmount;
    private string _remarks;

    #endregion
    #region Properties

    public int LendingId
    {
        get => _lendingId;
        set
        {
            if (value == _lendingId) return;
            _lendingId = value;
            _lending.LendingId = value;
            OnPropertyChanged(nameof(LendingId));
        }
    }

    public int BookId
    {
        get => _bookId;
        set
        {
            if (value == _bookId) return;
            _bookId = value;
            _lending.BookId = value;
            OnPropertyChanged(nameof(BookId));
        }
    }

    public string Isbn
    {
        get => _isbn;
        set
        {
            if (value == _isbn) return;
            _isbn = value;
            OnPropertyChanged(nameof(Isbn));
        }
    }

    public int MemberId
    {
        get => _memberId;
        set
        {
            if (value == _memberId) return;
            _memberId = value;
            _lending.MemberId = value;
            OnPropertyChanged(nameof(MemberId));
        }
    }

    public string MemberName
    {
        get => _memberName;
        set
        {
            if (value == _memberName) return;
            _memberName = value;
            _lending.MemberName = value;
            OnPropertyChanged(nameof(MemberName));
        }
    }

    public DateTime DateTimeBorrowed
    {
        get => _dateTimeBorrowed;
        set
        {
            if (value == _dateTimeBorrowed) return;
            _dateTimeBorrowed = value;
            _lending.DateTimeBorrowed = value;
            OnPropertyChanged(nameof(DateTimeBorrowed));
        }
    }

    public string UsernameLent
    {
        get => _usernameLent;
        set
        {
            if (value == _usernameLent) return;
            _usernameLent = value;
            _lending.UsernameLent = value;
            OnPropertyChanged(nameof(UsernameLent));
        }
    }


    public string Remarks
    {
        get => _remarks;
        set
        {
            if (value == _remarks) return;
            _remarks = value;
            OnPropertyChanged(nameof(Remarks));
        }
    }

    #endregion
    
    #region ICommands

    public ICommand EditionCommand { get; }
    public ICommand GoBackCommand { get; }

    #endregion

    #region Methods

    private void FillModel()
    {
        if (_mode == "Edit")
        {
            _lendingId = _lending.LendingId;
            _bookId = _lending.BookId;
            _bookName = _lending.BookName;
            _memberId = _lending.MemberId;
            _memberName = _lending.MemberName;
            _dateTimeBorrowed = _lending.DateTimeBorrowed;
            _usernameLent = _lending.UsernameLent;
            _remarks = _lending.Remarks;
        }
        else
        {
            DateTimeBorrowed = DateTime.Now;
            UsernameLent = _currentUser.FirstName;
        }
        
    }

    private async void ExecuteEditionCommand(object obj)
    {
        if (_mode == "Add")
        {
            _lending = new LendingModel
            {
                LendingId = 0,
                BookId =  _bookRepository.GetById(TryConvert.ToInt32(_isbn,0)).Id,
                BookName = _bookName,
                MemberId = _memberId,
                MemberName = _memberName,
                DateTimeBorrowed = _dateTimeBorrowed,
                UsernameLent = _usernameLent,
                Remarks = _remarks
            };


            await _lendingRepository.Add(_lending, _currentUser);
            GoBackCommand.Execute(null);
        }
        else
        {
            var book = Task.Run(() => _bookRepository.GetById(TryConvert.ToLong(_isbn,0))).Result;
            _lending = new LendingModel
            {
                LendingId = 0,
                BookId = book.Id ,
                BookName = book.Name,
                MemberId = _memberId,
                MemberName = _memberName,
                DateTimeBorrowed = _dateTimeBorrowed,
                UsernameLent = _usernameLent,
                Remarks = _remarks
            };
            await _lendingRepository.Edit(_lending, _lendingId);
            GoBackCommand.Execute(null);
        }
    }

    #endregion

    #region Constructor

    public LendingsFieldsViewModel(LendingModel lending, string mode, NavigationStore navigationStore,
        UserModel currentModel)
    {
        _mode = mode;
        _lending = lending ?? new LendingModel();
        _currentUser = new UserModel();
        _currentUser = currentModel;
        GoBackCommand = new GoLendingsCommand(null,
            new NavigationService<LendingsViewModel>(navigationStore,
                () => new LendingsViewModel(navigationStore, _currentUser)));
        _lendingRepository = new LendingRepository();
        _bookRepository = new BookRepository();
        EditionCommand = new ViewModelCommand(ExecuteEditionCommand);

        
            FillModel();
    }

    #endregion
}