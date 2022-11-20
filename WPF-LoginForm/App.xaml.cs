using System.Windows;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels;
using WPFBiblioteca.Views;

namespace WPFBiblioteca;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected void ApplicationStart(object sender, StartupEventArgs e)
    {
        var navigationStore = new NavigationStore();
        navigationStore.CurrentViewModel = new BooksViewModel(navigationStore); //Change navigationStore for home
        var loginView = new LoginView();
        loginView.Show();
        loginView.IsVisibleChanged += (s, ev) =>
        {
            if (loginView.IsVisible == false && loginView.IsLoaded)
            {
                var mainView = new MainView
                {
                    DataContext = new MainViewModel(navigationStore)
                };
                mainView.Show();
                loginView.Close();
            }
        };
    }
}