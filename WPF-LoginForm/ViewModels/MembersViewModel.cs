using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFBiblioteca.Commands;
using WPFBiblioteca.Models;
using WPFBiblioteca.Repositories;
using WPFBiblioteca.Services;
using WPFBiblioteca.Stores;
using WPFBiblioteca.ViewModels.Fields;

namespace WPFBiblioteca.ViewModels
{
    public class MembersViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<MemberModel> _collectionMembers;
        private MemberModel _member;
        private readonly IMemberRepository _membersRepository;
        private NavigationStore _navigationStore;
        private string _errorCode;
        private bool _canDelete;


        #endregion

        #region ICommands
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand EditCommand { get; }


        #endregion

        #region Constructor

        public MembersViewModel(NavigationStore navigationStore)
        {
            _canDelete = false;
            _errorCode = String.Empty;
            _navigationStore = navigationStore;
            _membersRepository = new MemberRepository();
            _member = new MemberModel();
            _collectionMembers = new ObservableCollection<MemberModel>();
            RemoveCommand = new ViewModelCommand(ExecuteRemoveCommand, CanExecuteRemove);
            AddCommand = new NavigateCommand<MembersFieldsViewModel>(
                new NavigationService<MembersFieldsViewModel>(navigationStore,
                    () => new MembersFieldsViewModel(null, "Add", navigationStore)));
            EditCommand = new NavigateCommand<MembersFieldsViewModel>(
                new NavigationService<MembersFieldsViewModel>(navigationStore,
                    () => new MembersFieldsViewModel(_member, "Edit", navigationStore)));
            ExecuteGetAllCommand(null);
        }

        #region Methods

        private bool CanExecuteRemove(object obj)
        {
            return (_member != null);
        }

        private void ExecuteRemoveCommand(object obj)
        {
            _membersRepository.Delete(_member.MemberId);
            ExecuteGetAllCommand(null);
        }

        private async void ExecuteGetAllCommand(object o)
        {
            CollectionMembers = new ObservableCollection<MemberModel>(await _membersRepository.GetByAll());
            
            _errorCode = _membersRepository.GetError();
            
        }

        #endregion

        #endregion

        #region Properties

        public ObservableCollection<MemberModel> CollectionMembers
        {
            get => _collectionMembers;
            set
            {
                _collectionMembers = value;
                OnPropertyChanged(nameof(CollectionMembers));
            }
        }

        public MemberModel Member
        {
            get => _member;
            set
            {
                if (Equals(value, _member)) return;
                _member = value;
                 CanDelete = _member != null;
                OnPropertyChanged(nameof(Member));
            }
        }

        public bool CanDelete
        {
            get => _canDelete;
            set
            {
                if (value == _canDelete) return;
                _canDelete = value;
                OnPropertyChanged(nameof(CanDelete));
            }
        }

        #endregion

       
    }
}
