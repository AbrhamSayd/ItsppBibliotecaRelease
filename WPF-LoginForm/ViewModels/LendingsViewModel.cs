using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation.Collections;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels.Fields;

namespace WPFBiblioteca.ViewModels;

public class LendingsViewModel : ViewModelBase
{
    #region fields

    private ObservableCollection<LendingModel> _activeLendingsCollection;
    private ObservableCollection<LendingReturnedModel> _lendingsUnActiveCollection;
    private LendingModel _lendingModel;
    private readonly ILendingRepository _lendingRepository;
    private readonly ILendingReturnedRepository _lendingReturnedRepository;
    private NavigationStore _navigationStore;
    private string _errorCode;
    private bool _canDelete;
    private bool _isChecked;
    private bool _activeLendings;
    private bool _unActiveLendings;
    private string _activeCollection;
    private readonly UserModel _currentUser;

    #endregion

    #region ICommands

    public ICommand NavigateAddCommand { get; }
    public ICommand RemoveCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand SwitchTableSLendings { get; }

    #endregion

    #region Methods

    private async void ExecuteGetAllCommand()
    {
       
        if (_activeLendings) ActiveLendingsCollection = new ObservableCollection<LendingModel>(await Task.Run(() => _lendingRepository.GetByAll()));
        if (_unActiveLendings)
            UnActiveLendingsCollection =
                new ObservableCollection<LendingReturnedModel>(Task.Run(() => _lendingReturnedRepository.GetByAll()).Result);

        _errorCode = _lendingRepository.GetError();
    }

    private bool CanExecuteRemoveRowCommand(object obj)
    {
        return _lendingModel != null;
    }

    private void ExecuteRemoveRowCommand(object obj)
    {
        _lendingRepository.Delete(_lendingModel.LendingId);
        _lendingReturnedRepository.Insert(_lendingModel, _currentUser);
        ExecuteGetAllCommand();
    }

    private void SwitchItemSource(object obj)
    {
        switch (_isChecked)
        {
            case true:
                UnActiveLendings = true;
                ActiveLendings = false;
                ActiveCollection = "Prestamos devueltos";
                ExecuteGetAllCommand();
                break;
            case false:
                UnActiveLendings = false;
                ActiveLendings = true;
                ActiveCollection = "Prestamos Activos";
                ExecuteGetAllCommand();
                break;
        }
    }

    #endregion

    #region Properties

    

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

    public LendingModel LendingModel
    {
        get => _lendingModel;
        set
        {
            _lendingModel = value;
            CanDelete = _lendingModel != null;
            OnPropertyChanged(nameof(LendingModel));
        }
    }

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            _isChecked = value;
            OnPropertyChanged(nameof(IsChecked));
        }
    }


    public ObservableCollection<LendingModel> ActiveLendingsCollection
    {
        get => _activeLendingsCollection;
        set
        {
            if (Equals(value, _activeLendingsCollection)) return;
            _activeLendingsCollection = value;
            OnPropertyChanged(nameof(ActiveLendingsCollection));
            ;
        }
    }

    public ObservableCollection<LendingReturnedModel> UnActiveLendingsCollection
    {
        get => _lendingsUnActiveCollection;
        set
        {
            if (Equals(value, _lendingsUnActiveCollection)) return;
            _lendingsUnActiveCollection = value;
            OnPropertyChanged(nameof(UnActiveLendingsCollection));
            ;
        }
    }
    public bool ActiveLendings
    {
        get => _activeLendings;
        set
        {
            if (value == _activeLendings) return;
            _activeLendings = value;
            OnPropertyChanged(nameof(ActiveLendings));
        }
    }

    public bool UnActiveLendings
    {
        get => _unActiveLendings;
        set
        {
            if (value == _unActiveLendings) return;
            _unActiveLendings = value;
            OnPropertyChanged(nameof(UnActiveLendings));
        }
    }

    public string ActiveCollection
    {
        get => _activeCollection;
        set
        {
            if (value == _activeCollection) return;
            _activeCollection = value;
            OnPropertyChanged(nameof(ActiveCollection));
        }
    }

    #endregion

    #region Constructor

    public LendingsViewModel(NavigationStore navigationStore, UserModel currentUser)
    {
        _currentUser = currentUser;
        _canDelete = false;
        _isChecked = false;
        _errorCode = string.Empty;
        _activeLendings = true;
        _activeCollection = "Prestamos Activos";
        _unActiveLendings = false;
        _navigationStore = navigationStore;
        _lendingRepository = new LendingRepository();
        _lendingReturnedRepository = new LendingReturnedRepository();
        _lendingModel = new LendingModel();
        _activeLendingsCollection = new ObservableCollection<LendingModel>();
        NavigateAddCommand = new NavigateCommand<LendingsFieldsViewModel>(
            new NavigationService<LendingsFieldsViewModel>(navigationStore,
                () => new LendingsFieldsViewModel(null, "Add", navigationStore, _currentUser)));
        SwitchTableSLendings = new ViewModelCommand(SwitchItemSource);
        EditCommand = new NavigateCommand<LendingsFieldsViewModel>(
            new NavigationService<LendingsFieldsViewModel>(navigationStore,
                () => new LendingsFieldsViewModel(_lendingModel, "Edit", navigationStore, _currentUser)));
        RemoveCommand = new ViewModelCommand(ExecuteRemoveRowCommand, CanExecuteRemoveRowCommand);
        ExecuteGetAllCommand();
    }

    #endregion
}