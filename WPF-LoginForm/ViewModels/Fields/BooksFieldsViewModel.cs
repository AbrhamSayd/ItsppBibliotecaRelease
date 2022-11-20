using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;

namespace WPFBiblioteca.ViewModels.Fields
{
    public class BooksFieldsViewModel : ViewModelBase
    {
        #region Fields

        private readonly ErrorsViewModel _errorsViewModel;
        private BookModel _book;
        private CategoryModel _category;
        private int _id;
        private string _isbn;
        private int _staticId;
        private string _name;
        private string _author;
        private string _editorial;
        private string _publishedYear;
        private int _stock;
        private string _color;
        private string _location;
        private string _remarks;
        private readonly string _mode;
        private int _categoryId;
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private ObservableCollection<CategoryModel> _categories;

        #endregion

        #region Icommands

        public ICommand EditionCommand { get; }
        public ICommand GoBackCommand { get; }

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
            EditionCommand = new ViewModelCommand(ExecuteEditionCommand);
            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            ExecuteGetCategories(null);
            if (mode == "Edit")
                FillModel();
        }



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
            _publishedYear = _book.PublishedYear;
            _stock = _book.Stock;
            _color = _book.Color;
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
                    Color = _color,
                    CategoryId = _categoryId,
                    Location = _location,
                    Remarks = _remarks
                };

                await _bookRepository.Add(_book,_categoryId);
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
                {
                    _errorsViewModel.AddError(nameof(Id), "Numero de empleado invalido");
                }

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
                if (_isbn.Length is <= 9 or > 14)
                {
                    _errorsViewModel.AddError(nameof(Isbn), "Isbn invalido");
                }

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
                if (String.IsNullOrEmpty(_name) || _name.Length < 6)
                {
                    _errorsViewModel.AddError(nameof(Name), "Nombre de libro invalido o muy corto");
                }

                OnPropertyChanged(nameof(Isbn));
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
                if (String.IsNullOrEmpty(_author) || _author.Length < 6)
                {
                    _errorsViewModel.AddError(nameof(Author), "Nombre de Autor invalido o muy corto");
                }

                OnPropertyChanged(nameof(Isbn));
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
                if (String.IsNullOrEmpty(_editorial) || _editorial.Length < 6)
                {
                    _errorsViewModel.AddError(nameof(Editorial), "Nombre de editorial invalido o muy corto");
                }

                OnPropertyChanged(nameof(Isbn));
            }
        }

        public string PublishedYear
        {
            get => _publishedYear;
            set
            {
                _publishedYear = value;
                _book.PublishedYear = value;
                _errorsViewModel.ClearErrors(nameof(PublishedYear));
                if (String.IsNullOrEmpty(_publishedYear) || _publishedYear.Length < 6)
                {
                    _errorsViewModel.AddError(nameof(Name), "Fecha invalida");
                }

                OnPropertyChanged(nameof(Isbn));
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
                {
                    _errorsViewModel.AddError(nameof(Name), "Existencia invalida");
                }

                OnPropertyChanged(nameof(Isbn));
            }
        }

        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                _book.Color = value;
                _errorsViewModel.ClearErrors(nameof(Color));
                if (String.IsNullOrEmpty(_color) || _color.Length < 4)
                {
                    _errorsViewModel.AddError(nameof(Name), "Color invalido, intente denuevo");
                }

                OnPropertyChanged(nameof(Color));
            }
        }

        public CategoryModel Category
        {
            get => _category;
            set
            {
                _category = value;
                _book.CategoryId = _category.CategoryId;
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
                if (String.IsNullOrEmpty(_location) || _location.Length < 4)
                {
                    _errorsViewModel.AddError(nameof(Location), "Locacion invalida");
                }

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
                if (String.IsNullOrEmpty(_remarks))
                {
                    _errorsViewModel.AddError(nameof(Remarks), "Notas invalidas, intente denuevo");
                }

                OnPropertyChanged(nameof(Location));
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
}