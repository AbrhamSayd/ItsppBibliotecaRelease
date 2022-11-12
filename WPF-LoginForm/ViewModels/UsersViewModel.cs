using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels.Fields;
using WPFBiblioteca.Views.FieldsViews;


namespace WPFBiblioteca.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        //fields
        private readonly IUserRepository _userRepository;
        
        private readonly NavigationStore _navigationStore;
        

        //Icommands
        
        public ICommand NavigateAddCommand { get; }
        
        //constructor
        public UsersViewModel(NavigationStore navigationStore)
        {
            NavigateAddCommand = new NavigateCommand<UserFieldsViewModel>(new NavigationService<UserFieldsViewModel>(navigationStore, () => new UserFieldsViewModel(navigationStore)));
        }

        //methods

        //Properties
    }
}
