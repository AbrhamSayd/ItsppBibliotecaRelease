using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Models.ComboBoxModels;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Repositories.ComboBox;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;

namespace WPFBiblioteca.ViewModels.Fields;

public class BooksFieldsViewModel : ViewModelBase
{
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
        ExecuteGetCategories(null);
        ExecuteGetColors(null);
        if (mode == "Edit") FillModel();
    }

    #endregion

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
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IColorRepository _colorRepository;
    private ObservableCollection<CategoryModel> _categories;
    private ObservableCollection<ColorModel> _colors;

    #endregion

    #region Icommands

    public ICommand EditionCommand { get; }
    public ICommand GoBackCommand { get; }

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


    private void ExecuteEditionCommand(object obj)
    {
        if (_mode == "Add")
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
                ColorId = _colorId,
                CategoryId = _categoryId,
                Location = _location,
                Remarks = _remarks
            };

            _bookRepository.Add(_book, _categoryId);
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
            _bookRepository.Edit(_book, _staticId);
            GoBackCommand.Execute(null);
        }
    }

    private async void ExecuteGetCategories(object obj)
    {
        Categories = new ObservableCollection<CategoryModel>(await _categoryRepository.GetByAll());
        Category = _categories[CategoryId - 1];
    }

    private async void ExecuteGetColors(object obj)
    {
        Colors = new ObservableCollection<ColorModel>(await _colorRepository.GetByAll());
        Color = _colors[ColorId - 1];
    }

    private bool CanExecuteEdition(object obj)
    {
        if (Category == null || Color == null || string.IsNullOrEmpty(LocationA) ||
            string.IsNullOrEmpty(LocationB)) return false;
        return true;
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

    #endregion
}