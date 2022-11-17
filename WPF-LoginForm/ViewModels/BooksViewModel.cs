using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Stores;

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





        #endregion

        #region Constructor

        public BooksViewModel(NavigationStore navigationStore)
        {
            _errorCode = string.Empty;
            _navigationStore = navigationStore;
            _bookRepository = new BookRepository();
            _bookModel = new BookModel();
            _collectionBooks = new ObservableCollection<BookModel>();

            ExecuteGetAllCommand(null);
        }

        #endregion

        #region Methods

        private async void ExecuteGetAllCommand(object o)
        {
            CollectionBooks = new ObservableCollection<BookModel>(await _bookRepository.GetByAll());
            _errorCode = _bookRepository.GetError();
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

        #endregion
    }
}