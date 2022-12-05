using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Helpers;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels.Fields;

namespace WPFBiblioteca.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region fields

        private UserModel _currentUser;
        private NavigationStore _navigationStore;
        private readonly UserRepository _userRepository;
        private readonly BookRepository _bookRepository;
        private readonly LendingRepository _lendingRepository;
        private string _username;
        private bool _visibility;
        private string _title;
        private string _folio;
        private string _bookName;
        #endregion

        #region ICommands

        public ICommand NavigateAddCommand { get; }
        public ICommand ReturnBook { get; }
        public ICommand SearchBook { get; }
        public ICommand CancelReturnCommand { get; }
        public ICommand AcceptReturnCommand { get; }

        #endregion

        #region Methods

        private async void LoadBookName(object obj)
        {
            if (await _lendingRepository.LendingExist(ValidationHelper.TryConvert.ToInt32(_folio, 0)))
            {
                
                var lending = await _lendingRepository.GetById(ValidationHelper.TryConvert.ToInt32(_folio, 0));
                Username = lending.MemberName;
                BookName = lending.BookName;
            }
            else
            {
                BookName = "Prestamo no encontrado";
            }
            
        }
        private void LoadCurrentUserData()
        {
            if (Thread.CurrentPrincipal == null) return;
            var user = _userRepository.GetByUsername(Thread.CurrentPrincipal.Identity!.Name);
            if (user != null)
            {
                _currentUser = user;
            }
        }
        

        private void ExecuteReturnBook(object obj)
        {
            Task.Run(() => _lendingRepository.Delete(ValidationHelper.TryConvert.ToInt32(_folio,0)));
            Visibility = false;
        }
        private void ShowReturn(object obj)
        {
            _folio = string.Empty;
            Visibility = true;
            
            
        }
        private void HideReturn(object obj)
        {
            _folio = string.Empty;
            Visibility = true;


        }
        private bool canReturn(object obj)
        {
            var canDelete = !(string.IsNullOrEmpty(_bookName) && _bookName == "Prestamo no encontrado");

            return canDelete;

        }
        #endregion

        #region Properties

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

        public string Folio
        {
            get => _folio;
            set
            {
                if (value == _folio) return;
                _folio = value;
                OnPropertyChanged(nameof(Folio));
            }
        }

        public string BookName
        {
            get => _bookName;
            set
            {
                if (value == _bookName) return;
                _bookName = value;
                OnPropertyChanged(nameof(BookName));
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

        public string Username
        {
            get => _username;
            set
            {
                if (value == _username) return;
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        #endregion
        #region Constructor

        public HomeViewModel(NavigationStore navigationStore, UserModel currentUser)
        {
            _currentUser = currentUser;
            _userRepository = new UserRepository();
            _bookRepository = new BookRepository();
            _lendingRepository = new LendingRepository();
            _navigationStore = navigationStore;
            _visibility = false;
            if (currentUser == null)
            {
                LoadCurrentUserData();
            }

            CancelReturnCommand = new ViewModelCommand(HideReturn);
            AcceptReturnCommand = new ViewModelCommand(ExecuteReturnBook,canReturn);
            SearchBook = new ViewModelCommand(LoadBookName);
            ReturnBook = new ViewModelCommand(ShowReturn);
            NavigateAddCommand = new NavigateCommand<LendingsFieldsViewModel>(
                new NavigationService<LendingsFieldsViewModel>(navigationStore,
                    () => new LendingsFieldsViewModel(null, "Add", navigationStore, _currentUser)));
        }

        

        #endregion
    }
}