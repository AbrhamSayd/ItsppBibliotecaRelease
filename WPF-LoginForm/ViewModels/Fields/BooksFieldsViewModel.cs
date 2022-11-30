using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Models.ComboBoxModels;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Repositories.ComboBox;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WPFBiblioteca.ViewModels.Fields;

public class BooksFieldsViewModel : ViewModelBase
{
    

    #region Fields

    private BookModel _book;
    private CategoryModel _category;
    private ColorModel _color;
    private int _colorId;
    private int _id;
    private string _isbn;
    private int _staticId;
    private string _name;
    private string _author;
    private string _editorial;
    private int _publishedYear;
    private int _stock;
    private string _location;
    private string _locationA;
    private string _locationB;
    private string _remarks;
    private readonly string _mode;
    private int _categoryId;
    private string _element;
    private string _title;
    private bool _visibility;
    private string _errorFocus;
    private string _errorCode;
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IColorRepository _colorRepository;
    private ObservableCollection<BookModel> _books;
    private ObservableCollection<CategoryModel> _categories;
    private ObservableCollection<ColorModel> _colors;

    #endregion

    #region Icommands

    public ICommand EditionCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand AcceptCommand { get; }

    #endregion

    #region Methods

    private void FillModel()
    {
        _staticId = _book.Id;
        _id = _book.Id;
        _isbn = _book.Isbn;
        _name = _book.Name;
        _author = _book.Author;
        _editorial = _book.Editorial;
        if (_book.PublishedYear != null) _publishedYear = (int)_book.PublishedYear;
        _stock = _book.Stock;
        _colorId = _book.ColorId;
        _categoryId = _book.CategoryId;
        _location = _book.Location;
        _remarks = _book.Remarks;
        _locationA = _book.Location.Split("-")[0];
        _locationB = _book.Location.Split("-")[1];
    }


    private async void ExecuteEditionCommand(object obj)
    {
        var isDuplicate = false;
        if (_mode == "Add")
        {
            foreach (var book in Books)
            {
                if (_isbn != book.Isbn) continue;
                Element = "Isbn duplicado, Intente con otro porfavor o verifique";
                Title = "Dato duplicado";
                Visibility = true;
                isDuplicate = true;
            }

            if (isDuplicate) return;
            _book = new BookModel
            {
                Id = _id,
                Isbn = _isbn,
                Name = _name,
                Author = _author,
                Editorial = _editorial,
                PublishedYear = _publishedYear,
                Stock = _stock,
                ColorId = Color.ColorId,
                CategoryId = Category.CategoryId,
                Location = _location,
                Remarks = _remarks
            };
            await _bookRepository.Add(_book, _staticId);
            GoBackCommand.Execute(null);

        }
        else
        {
            _book = new BookModel
            {
                Id = _id,
                Isbn = _isbn,
                Name = _name,
                Author = _author,
                Editorial = _editorial,
                PublishedYear = _publishedYear,
                Stock = _stock,
                ColorId = Color.ColorId,
                CategoryId = Category.CategoryId,
                Location = _location,
                Remarks = _remarks
            };
           await _bookRepository.Edit(_book, _staticId);
           _errorCode = _bookRepository.GetError();
            GoBackCommand.Execute(null);
        }
    }

    private async void ExecuteGetCategories(object obj)
    {
        Categories = new ObservableCollection<CategoryModel>(await _categoryRepository.GetByAll());
        if (_mode == "Edit")
        {
            Category = _categories[CategoryId - 1];
        }
        
    }

    private async void ExecuteGetColors(object obj)
    {
        Colors = new ObservableCollection<ColorModel>(await _colorRepository.GetByAll());
        if (_mode == "Edit")
        {
            Color = _colors[ColorId - 1];
        }
        
    }

    private bool CanExecuteEdition(object obj)
    {
        if (Category == null || Color == null || string.IsNullOrEmpty(LocationA) ||
            string.IsNullOrEmpty(LocationB)) return false;
        return true;
    }
    private async void ExecuteGetAllCommand(object o)
    {
        Books = new ObservableCollection<BookModel>(await _bookRepository.GetByAll());
        _errorCode = _bookRepository.GetError();
    }

    private void ExecuteAcceptCommand(object obj)
    {
        switch (_errorFocus)
        {
            case "Id":
                Id = 0;
                break;
        }

        Visibility = false;
    }

    #endregion

    #region Properties

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            _book.Id = value;

            OnPropertyChanged(nameof(Id));
        }
    }

    public string Isbn
    {
        get => _isbn;
        set
        {
            _isbn = value;
            _book.Isbn = value;

            OnPropertyChanged(nameof(Isbn));
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _book.Name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public string Author
    {
        get => _author;
        set
        {
            _author = value;
            _book.Author = value;
            OnPropertyChanged(nameof(Author));
        }
    }

    public string Editorial
    {
        get => _editorial;
        set
        {
            _editorial = value;
            _book.Editorial = value;
            OnPropertyChanged(nameof(Editorial));
        }
    }

    public int PublishedYear
    {
        get => _publishedYear;
        set
        {
            _publishedYear = value;
            OnPropertyChanged(nameof(PublishedYear));
        }
    }

    public int Stock
    {
        get => _stock;
        set
        {
            _stock = value;
            _book.Stock = value;
            OnPropertyChanged(nameof(Stock));
        }
    }

    public int ColorId
    {
        get => _colorId;
        set
        {
            _colorId = value;
            _book.ColorId = value;
            OnPropertyChanged(nameof(ColorId));
        }
    }

    public int CategoryId
    {
        get => _categoryId;
        set
        {
            if (value == _categoryId) return;
            _categoryId = value;
            OnPropertyChanged(nameof(CategoryId));
        }
    }

    public ColorModel Color
    {
        get => _color;
        set
        {
            _color = value;
            OnPropertyChanged(nameof(Color));
        }
    }

    public CategoryModel Category
    {
        get => _category;
        set
        {
            _category = value;
            OnPropertyChanged(nameof(Category));
        }
    }

    public string Location
    {
        get => _location;
        set
        {
            _location = _locationA + "-" + _locationB;
            _book.Location = _location;
            //if (string.IsNullOrEmpty(_location) || _location.Length < 4)
            //    _errorsViewModel.AddError(nameof(Location), "Locacion invalida");
            OnPropertyChanged(nameof(Location));
        }
    }

    public string Remarks
    {
        get => _remarks;
        set
        {
            _remarks = value;
            _book.Remarks = value;
            //if (string.IsNullOrEmpty(_remarks))
            //    _errorsViewModel.AddError(nameof(Remarks), "Notas invalidas, intente denuevo");

            OnPropertyChanged(nameof(Remarks));
        }
    }

    public string LocationA
    {
        get => _locationA;
        set
        {
            _locationA = value;
            _location = value + "-" + _locationB;
            OnPropertyChanged(nameof(LocationA));
        }
    }

    public string LocationB
    {
        get => _locationB;
        set
        {
            _locationB = value;
            _location = _locationA + '-' + _locationB;
            OnPropertyChanged(nameof(LocationB));
        }
    }

    public ObservableCollection<CategoryModel> Categories
    {
        get => _categories;
        set
        {
            _categories = value;
            OnPropertyChanged(nameof(Categories));
        }
    }

    public ObservableCollection<ColorModel> Colors
    {
        get => _colors;
        set
        {
            _colors = value;
            OnPropertyChanged(nameof(Colors));
        }
    }

    public ObservableCollection<BookModel> Books
    {
        get => _books;
        set
        {
            if (Equals(value, _books)) return;
            _books = value;
            OnPropertyChanged(nameof(Books));
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

    #endregion

    #region Constructor

    public BooksFieldsViewModel(BookModel book, string mode, NavigationStore navigationStore)
    {
        _mode = mode;
        _book = book ?? new BookModel();

        GoBackCommand = new GoBooksCommand(null,
            new NavigationService<BooksViewModel>(navigationStore,
                () => new BooksViewModel(navigationStore)));
        _bookRepository = new BookRepository();
        _categoryRepository = new CategoryRepository();
        _colorRepository = new ColorRepository();
        EditionCommand = new ViewModelCommand(ExecuteEditionCommand, CanExecuteEdition);
        AcceptCommand = new ViewModelCommand(ExecuteAcceptCommand);
        ExecuteGetCategories(null);
        ExecuteGetColors(null);
        ExecuteGetAllCommand(null);
        if (mode == "Edit") FillModel();
    }

    #endregion
}