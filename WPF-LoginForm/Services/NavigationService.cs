using System;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels;

namespace WPFBiblioteca.Services;

public class NavigationService<TViewModel>
    where TViewModel : ViewModelBase
{
    private readonly Func<TViewModel> _createViewModel;
    private readonly NavigationStore _navigationStore;

    public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }

    public NavigationService(NavigationStore navigationStore, Func<LendingsViewModel> createViewModel)
    {
    }

    public void Navigate()
    {
         _navigationStore.CurrentViewModel = _createViewModel();
    }
}