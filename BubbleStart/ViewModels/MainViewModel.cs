using BubbleStart.Database;
using BubbleStart.Messages;
using BubbleStart.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BubbleStart.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Messenger.Default.Register<ChangeVisibilityMessage>(this, msg => { Visibility = msg.Visible ? Visibility.Visible : Visibility.Collapsed; });
            Messenger.Default.Register<LoginLogOutMessage>(this, async msg => { await ChangeViewModel(msg.Login); });
        }

        private async Task ChangeViewModel(bool login)
        {
            StartingRepository = new GenericRepository();
            if (login)
            {
                SelectedViewmodel = new MainUserControl_ViewModel(StartingRepository);
            }
            else
            {
                SelectedViewmodel = new LoginViewModel(StartingRepository);
            }
            await SelectedViewmodel.LoadAsync();
            RaisePropertyChanged(nameof(MenuVisibility));
        }

        #region Fields

        private IViewModel _SelectedViewmodel;

        private Visibility _Visibility;

        private GenericRepository StartingRepository;

        #endregion Fields

        #region Properties

        public Visibility MenuVisibility
        {
            get
            {
                if (Helpers.StaticResources.User != null && Helpers.StaticResources.User.Level <= 1)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public IViewModel SelectedViewmodel
        {
            get
            {
                return _SelectedViewmodel;
            }

            set
            {
                if (_SelectedViewmodel == value)
                {
                    return;
                }

                _SelectedViewmodel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the Visibility property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                return _Visibility;
            }

            set
            {
                if (_Visibility == value)
                {
                    return;
                }

                _Visibility = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        public async Task LoadAsync(GenericRepository startingRepository)
        {
            StartingRepository = startingRepository;
#if DEBUG
            Helpers.StaticResources.User = (await StartingRepository.GetAllAsync<User>(u => u.Id == 3)).First();
            RaisePropertyChanged(nameof(MenuVisibility));
#endif
            if (Helpers.StaticResources.User == null)
                SelectedViewmodel = new LoginViewModel(StartingRepository);//TODO
            else
                SelectedViewmodel = new MainUserControl_ViewModel(StartingRepository);//TODO
            await SelectedViewmodel.LoadAsync();
        }
    }
}