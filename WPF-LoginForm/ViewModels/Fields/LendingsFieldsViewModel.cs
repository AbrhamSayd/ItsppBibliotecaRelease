using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;

namespace WPFBiblioteca.ViewModels.Fields
{
    public class LendingsFieldsViewModel : ViewModelBase
    {
        #region Constructor

        public LendingsFieldsViewModel(LendingModel lending, string mode, NavigationStore navigationStore, UserModel currentModel)
        {
            _mode = mode;
            _lending = lending ?? new LendingModel();
            _currentUser = new UserModel();
            _currentUser = currentModel;
            GoBackCommand = new GoLendingsCommand(null,
                new NavigationService<LendingsViewModel>(navigationStore,
                    () => new LendingsViewModel(navigationStore, _currentUser)));
            _lendingRepository = new LendingRepository();
            EditionCommand = new ViewModelCommand(ExecuteEditionCommand);
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            if (mode == "Edit")
                FillModel();
        }

        #endregion

        #region Fields

        private readonly string _mode;
        private readonly ErrorsViewModel _errorsViewModel;
        private LendingModel _lending;
        private UserModel _currentUser;

        private readonly ILendingRepository _lendingRepository;
        private ObservableCollection<CategoryModel> _lendings;
        

        private int _lendingId;
        private int _bookId;
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

        #region ICommands

        public ICommand EditionCommand { get; }
        public ICommand GoBackCommand { get; }

        #endregion

        #region Methods

        private void FillModel()
        {
            _lendingId = _lending.LendingId;
            _bookId = _lending.BookId;
            _bookName = _lending.BookName;
            _memberId = _lending.MemberId;
            _memberName = _lending.MemberName;
            _dateTimeBorrowed = _lending.DateTimeBorrowed;
            _usernameLent = _lending.UsernameLent;
            _dateTimeReturned = _lending.DateTimeReturned;
            _usernameReturned = _lending.UsernameReturned;
            if (_lending.FinedAmount != null) _finedAmount = (int)_lending.FinedAmount;
            _remarks = _lending.Remarks;
        }

        private async void ExecuteEditionCommand(object obj)
        {
            if (_mode == "Add")
            {
                _lending = new LendingModel
                {
                    LendingId = _lendingId,
                    BookId = _bookId,
                    BookName = _bookName,
                    MemberId = _memberId,
                    MemberName = _memberName,
                    DateTimeBorrowed = _dateTimeBorrowed,
                    UsernameLent = _usernameLent,
                    DateTimeReturned = _dateTimeReturned,
                    UsernameReturned = _usernameReturned,
                    FinedAmount = _finedAmount,
                    Remarks = _remarks
                };


                await _lendingRepository.Add(_lending,_currentUser);
                GoBackCommand.Execute(null);
            }
            else
            {
                await _lendingRepository.Edit(_lending, _lendingId);
                GoBackCommand.Execute(null);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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

        public string BookName
        {
            get => _bookName;
            set
            {
                if (value == _bookName) return;
                _bookName = value;
                _lending.BookName = value;
                OnPropertyChanged(nameof(BookName));
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

        public DateTime DateTimeReturned
        {
            get => _dateTimeReturned;
            set
            {
                if (value == _dateTimeReturned) return;
                _dateTimeReturned = value;
                _lending.DateTimeReturned = value;
                OnPropertyChanged(nameof(DateTimeReturned));
            }
        }

        public string UsernameReturned
        {
            get => _usernameReturned;
            set
            {
                if (value == _usernameReturned) return;
                _usernameReturned = value;
                OnPropertyChanged(nameof(UsernameReturned));
            }
        }

        public int FinedAmount
        {
            get => _finedAmount;
            set
            {
                if (value == _finedAmount) return;
                _finedAmount = value;
                OnPropertyChanged(nameof(FinedAmount));
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

        public bool CanCreate => !HasErrors;
        public bool HasErrors => _errorsViewModel.HasErrors;

        #endregion
    }
}
