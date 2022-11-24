using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels.Fields;

namespace WPFBiblioteca.ViewModels;

public class BooksViewModel : ViewModelBase
{
    #region Constructor

    public BooksViewModel(NavigationStore navigationStore)
    {
        _canDelete = false;
        _errorCode = string.Empty;
        _navigationStore = navigationStore;
        _bookRepository = new BookRepository();
        _bookModel = new BookModel();
        _collectionBooks = new ObservableCollection<BookModel>();
        _title = "Eliminar libro";
        _element = null;
        _visibility = false;
        NavigateAddCommand = new NavigateCommand<BooksFieldsViewModel>(
            new NavigationService<BooksFieldsViewModel>(navigationStore,
                () => new BooksFieldsViewModel(null, "Add", navigationStore)));
        EditCommand = new NavigateCommand<BooksFieldsViewModel>(
            new NavigationService<BooksFieldsViewModel>(navigationStore,
                () => new BooksFieldsViewModel(_bookModel, "Edit", navigationStore)));
        RemoveCommand = new ViewModelCommand(ExecuteRemoveRowCommand, CanExecuteRemoveRowCommand);
        AcceptRemoveCommand = new ViewModelCommand(ExecuteRemove);
        CancelRemoveCommand = new ViewModelCommand(CancelRemove);
        ExecuteGetAllCommand(null);
    }

    #endregion

    #region fields

    private ObservableCollection<BookModel> _collectionBooks;
    private BookModel _bookModel;
    private readonly IBookRepository _bookRepository;
    private NavigationStore _navigationStore;
    private string _errorCode;
    private bool _canDelete;
    private string _title;
    private string _element;
    private bool _visibility;

    #endregion

    #region ICommands

    public ICommand NavigateAddCommand { get; }
    public ICommand RemoveCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand AcceptRemoveCommand { get; }
    public ICommand CancelRemoveCommand { get; }

    #endregion

    #region Methods

    private async void ExecuteGetAllCommand(object o)
    {
        CollectionBooks = new ObservableCollection<BookModel>(await _bookRepository.GetByAll());
        _errorCode = _bookRepository.GetError();
    }

    private bool CanExecuteRemoveRowCommand(object obj)
    {
        return _bookModel != null;
    }

    private void ExecuteRemoveRowCommand(object obj)
    {
        Element = "¿Estas seguro de borra este " + _bookModel.Name + " libro";
        Visibility = true;
    }

    private void ExecuteRemove(object obj)
    {
        Visibility = false;
        _bookRepository.Delete(_bookModel.Id);
        CollectionBooks.Remove(_bookModel);
    }

    private void CancelRemove(object obj)
    {
        Visibility = false;
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

    #endregion
}