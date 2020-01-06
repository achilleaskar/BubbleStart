using BubbleStart.Database;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
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
            Messenger.Default.Register<LoginLogOutMessage>(this, async (msg) => await ChangeViewModel(msg.Login));
        }

        private async Task ChangeViewModel(bool login)
        {
            if (login)
            {
                SelectedViewmodel = new MainUserControl_ViewModel(BasicDataManager);
               // await BasicDataManager.Refresh();
            }
            else
            {
                SelectedViewmodel = new LoginViewModel(BasicDataManager);
            }
            Messenger.Default.Send(new BasicDataManagerRefreshedMessage());

            RaisePropertyChanged(nameof(MenuVisibility));
        }

        #region Fields
        MyViewModelBase _SelectedViewmodel;

        private Visibility _Visibility;

        private GenericRepository StartingRepository;

        #endregion Fields

        #region Properties

        public Visibility MenuVisibility
        {
            get
            {
                if (StaticResources.User != null && StaticResources.User.Level <= 1)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public MyViewModelBase SelectedViewmodel
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

        BasicDataManager BasicDataManager;

        #endregion Properties

        public async Task LoadAsync(GenericRepository startingRepository)
        {
            try
            {
                StartingRepository = startingRepository;
                BasicDataManager = new BasicDataManager(StartingRepository);
#if DEBUG
                StaticResources.User = new User { Name = "admin", Id = 3, Level = 0 };
                RaisePropertyChanged(nameof(MenuVisibility));
#endif
                if (StaticResources.User == null)
                    SelectedViewmodel = new LoginViewModel(BasicDataManager);//TODO
                else
                {
                    SelectedViewmodel = new MainUserControl_ViewModel(BasicDataManager);//TODO
                }
                await BasicDataManager.LoadAsync();
            }
            catch (Exception ex)
            {

                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }
    }
}