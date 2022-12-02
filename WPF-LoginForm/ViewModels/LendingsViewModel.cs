using System.Collections.ObjectModel;
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
    #region Constructor

    public LendingsViewModel(NavigationStore navigationStore, UserModel currentUser)
    {
        _currentUser = currentUser;
        _canDelete = false;
        _errorCode = string.Empty;
        _navigationStore = navigationStore;
        _lendingRepository = new LendingRepository();
        _lendingModel = new LendingModel();
        _collectionLendings = new ObservableCollection<LendingModel>();
        NavigateAddCommand = new NavigateCommand<LendingsFieldsViewModel>(
            new NavigationService<LendingsFieldsViewModel>(navigationStore,
                () => new LendingsFieldsViewModel(null, "Add", navigationStore, _currentUser)));

        EditCommand = new NavigateCommand<LendingsFieldsViewModel>(
            new NavigationService<LendingsFieldsViewModel>(navigationStore,
                () => new LendingsFieldsViewModel(_lendingModel, "Edit", navigationStore, _currentUser)));
        RemoveCommand = new ViewModelCommand(ExecuteRemoveRowCommand, CanExecuteRemoveRowCommand);
        ExecuteGetAllCommand(null);
    }

    #endregion

    #region fields

    private ObservableCollection<LendingModel> _collectionLendings;
    private LendingModel _lendingModel;
    private readonly ILendingRepository _lendingRepository;
    private NavigationStore _navigationStore;
    private string _errorCode;
    private bool _canDelete;
    private readonly UserModel _currentUser;
    private bool _isChecked;
    private string _tableNameSelected;

    #endregion


    #region ICommands

    public ICommand NavigateAddCommand { get; }
    public ICommand RemoveCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand SwitchTableSLendings { get; }

    #endregion

    #region Methods

    private async void ExecuteGetAllCommand(object o)
    {
        CollectionLendings = new ObservableCollection<LendingModel>(await _lendingRepository.GetByAll());
        _errorCode = _lendingRepository.GetError();
    }

    private bool CanExecuteRemoveRowCommand(object obj)
    {
        return _lendingModel != null;
    }

    private void ExecuteRemoveRowCommand(object obj)
    {
        _lendingRepository.Delete(_lendingModel.LendingId);
        ExecuteGetAllCommand(null);
    }

    #endregion

    #region Properties

    public ObservableCollection<LendingModel> CollectionLendings
    {
        get => _collectionLendings;
        set
        {
            _collectionLendings = value;
            OnPropertyChanged(nameof(CollectionLendings));
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

    public string TableNameSelected
    {
        get => _tableNameSelected;
        set 
        {
            _tableNameSelected = value;
            OnPropertyChanged(nameof(TableNameSelected));
        }
    }

    #endregion
}