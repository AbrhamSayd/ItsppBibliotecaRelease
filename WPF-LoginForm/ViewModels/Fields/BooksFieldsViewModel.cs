using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    #region Fields

    private readonly ErrorsViewModel _errorsViewModel;
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
    private string _remarks;
    private readonly string _mode;
    private int _categoryId;
    private DateTime _date;
    private DateTime _selectedDate;
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IColorRepository _colorRepository;
    private ObservableCollection<CategoryModel> _categories;

    #endregion
    #region Constructor

    public BooksFieldsViewModel(BookModel book, string mode, NavigationStore navigationStore)
    {
        _mode = mode;
        _book = book ?? new BookModel();
        _date = new DateTime();
        _date = DateTime.Now;
        GoBackCommand = new GoBooksCommand(null,
            new NavigationService<BooksViewModel>(navigationStore,
                () => new BooksViewModel(navigationStore)));
        _bookRepository = new BookRepository();
        _categoryRepository = new CategoryRepository();
        _colorRepository = new ColorRepository();
        EditionCommand = new ViewModelCommand(ExecuteEditionCommand);
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        ExecuteGetCategories(null);
        if (mode == "Edit")
            FillModel();
    }

    #endregion

    

    #region Icommands

    public ICommand EditionCommand { get; }
    public ICommand GoBackCommand { get; }

    #endregion

    #region Methods

    private void FillModel()
    {
        _staticId = Id;
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
    }

    private bool CanExecuteAddCommand(object obj)
    {
        return CanCreate;
    }

    private async void ExecuteEditionCommand(object obj)
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

            await _bookRepository.Add(_book, _categoryId);
            GoBackCommand.Execute(null);
        }
        else
        {
            await _bookRepository.Edit(_book, _staticId);
            GoBackCommand.Execute(null);
        }
    }

    private async void ExecuteGetCategories(object obj)
    {
        Categories = new ObservableCollection<CategoryModel>(await _categoryRepository.GetByAll());
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

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            
            _selectedDate = value;
            _publishedYear = value.Year;
            _book.PublishedYear = value.Year;
            OnPropertyChanged(nameof(SelectedDate));
        }
    }

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            _book.Id = value;
            _errorsViewModel.ClearErrors(nameof(Id));
            if (Math.Floor(Math.Log10(_id) + 1)
                is < 3 or > 8)
                _errorsViewModel.AddError(nameof(Id), "ID Invalido?");

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
            _errorsViewModel.ClearErrors(nameof(Isbn));
            if (_isbn.Length is <= 9 or > 14) _errorsViewModel.AddError(nameof(Isbn), "ISBN invalido");

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
            _errorsViewModel.ClearErrors(nameof(Name));
            if (string.IsNullOrEmpty(_name) || _name.Length < 6)
                _errorsViewModel.AddError(nameof(Name), "Nombre de libro invalido o muy corto");

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
            _errorsViewModel.ClearErrors(nameof(Author));
            if (string.IsNullOrEmpty(_author) || _author.Length < 6)
                _errorsViewModel.AddError(nameof(Author), "Nombre de Autor invalido o muy corto");

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
            _errorsViewModel.ClearErrors(nameof(Editorial));
            if (string.IsNullOrEmpty(_editorial) || _editorial.Length < 2)
                _errorsViewModel.AddError(nameof(Editorial), "Nombre de editorial invalido o muy corto");

            OnPropertyChanged(nameof(Editorial));
        }
    }

    public int PublishedYear
    {
        get => _publishedYear;
        set
        {
            _publishedYear = value;
            _errorsViewModel.ClearErrors(nameof(PublishedYear));
            if (_publishedYear<1500 || _publishedYear > _date.Year)
                _errorsViewModel.AddError(nameof(Name), "Fecha invalida");

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
            _errorsViewModel.ClearErrors(nameof(Stock));
            if (Math.Floor(Math.Log10(_id) + 1)
                is < 1 or > 3)
                _errorsViewModel.AddError(nameof(Name), "Existencia invalida");

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
            _location = value;
            _book.Location = value;
            _errorsViewModel.ClearErrors(nameof(Location));
            if (string.IsNullOrEmpty(_location) || _location.Length < 4)
                _errorsViewModel.AddError(nameof(Location), "Locacion invalida");

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
            _errorsViewModel.ClearErrors(nameof(Remarks));
            if (string.IsNullOrEmpty(_remarks))
                _errorsViewModel.AddError(nameof(Remarks), "Notas invalidas, intente denuevo");

            OnPropertyChanged(nameof(Remarks));
        }
    }

    public int CategoryId
    {
        get => _categoryId;
        set
        {
            _categoryId = value;
            OnPropertyChanged(nameof(CategoryId));
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

    public bool CanCreate => !HasErrors;
    public bool HasErrors => _errorsViewModel.HasErrors;

    #endregion
}