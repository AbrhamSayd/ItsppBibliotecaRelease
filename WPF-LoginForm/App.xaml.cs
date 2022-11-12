using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFBiblioteca.ViewModels;
using WPFBiblioteca.Views;

namespace WPFBiblioteca
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {



            var loginView = new LoginView();
            loginView.Show();
            loginView.IsVisibleChanged += (s, ev) =>
              {
                  if (loginView.IsVisible == false && loginView.IsLoaded)
                  {
                      var mainView = new MainView()
                      {
                          DataContext = new MainViewModel()
                      };
                      mainView.Show();
                      loginView.Close();
                  }
              };
        }
    }
}
