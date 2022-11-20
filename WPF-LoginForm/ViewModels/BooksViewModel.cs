
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels.Fields;

namespace WPFBiblioteca.ViewModels
{
    public class BooksViewModel : ViewModelBase
    {
        #region fields

        private ObservableCollection<BookModel> _collectionBooks;
        private BookModel _bookModel;
        private readonly IBookRepository _bookRepository;
        private NavigationStore  _navigationStore;
        private string _errorCode;
        private bool _canDelete;

        #endregion

        #region ICommands

        public ICommand NavigateAddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand EditCommand { get; }

        #endregion

        #region Constructor

        public BooksViewModel(NavigationStore navigationStore)
        {
            _canDelete = false;
            _errorCode = string.Empty;
            _navigationStore = navigationStore;
            _bookRepository = new BookRepository();
            _bookModel = new BookModel();
            _collectionBooks = new ObservableCollection<BookModel>();
            NavigateAddCommand = new NavigateCommand<BooksFieldsViewModel>(
                new                        NavigationService<BooksFieldsViewModel>(navigationStore,
                    () => new BooksFieldsViewModel(null, "Add", navigationStore)));
            EditCommand = new NavigateCommand<BooksFieldsViewModel>(
                new NavigationService<BooksFieldsViewModel>(navigationStore,
                    () => new BooksFieldsViewModel(_bookModel, "Edit", navigationStore)));
            RemoveCommand = new ViewModelCommand(ExecuteRemoveRowCommand,CanExecuteRemoveRowCommand);
            ExecuteGetAllCommand(null);
        }

        #endregion

        #region Methods

        private async void ExecuteGetAllCommand(object o)
        {
            CollectionBooks = new ObservableCollection<BookModel>(await _bookRepository.GetByAll());
            _errorCode = _bookRepository.GetError();
        }

        private bool CanExecuteRemoveRowCommand(object obj)
        {
            return (_bookModel != null) ;

        }

        private void ExecuteRemoveRowCommand(object obj)
        {
            _bookRepository.Delete(_bookModel.Id);
            ExecuteGetAllCommand(null);
        }

       
        #endregion

        #region Properties

        public ObservableCollection<BookModel> CollectionBooks
        {
            get => _collectionBooks;
            set
            {
                _collectionBooks = value;
                OnPropertyChanged(nameof(CollectionBooks));
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

        public BookModel BookModel
        {
            get => _bookModel;
            set
            {
                _bookModel = value;
                CanDelete = _bookModel != null;

                OnPropertyChanged(nameof(BookModel));
            }
        }

        #endregion
    }
}