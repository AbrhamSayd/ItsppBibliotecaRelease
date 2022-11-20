using WPFBiblioteca.Services;
using WPFBiblioteca.ViewModels;

namespace WPFBiblioteca.Commands;

public class NavigateCommand<TViewModel> : CommandBase
    where TViewModel : ViewModelBase
{
    private readonly NavigationService<TViewModel> _navigationStore;

    public NavigateCommand(NavigationService<TViewModel> navigationStore)
    {
        _navigationStore = navigationStore;
    }


    public override void Execute(object parameter)
    {
        _navigationStore.Navigate();
    }
}