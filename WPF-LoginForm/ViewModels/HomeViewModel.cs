using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFBiblioteca.Commands;
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

        private NavigationStore _navigationStore;

        #endregion

        #region Constructor

        public HomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        #endregion
    }
}
