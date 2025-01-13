using BubbleStart.Database;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BubbleStart.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Messenger.Reset();
            Messenger.Default.Register<ChangeVisibilityMessage>(this, msg => { Visibility = msg.Visible ? Visibility.Visible : Visibility.Collapsed; });
            Messenger.Default.Register<LoginLogOutMessage>(this, async msg => await ChangeViewModel(msg.Login));
        }

        private async Task ChangeViewModel(bool login)
        {
            if (login)
            {
                if (BasicDataManager.LogedOut)
                {
                   // BasicDataManager.Context.Dispose();
                    StartingRepository = new GenericRepository();
                    BasicDataManager = new BasicDataManager(StartingRepository);
                    await BasicDataManager.LoadAsync();
                    Mouse.OverrideCursor = Cursors.Arrow;
                    BasicDataManager.LogedOut = false;
                }
                SelectedViewmodel = new MainUserControl_ViewModel(BasicDataManager);
            }
            else
            {
                SelectedViewmodel = new LoginViewModel(BasicDataManager)
                {
                    IsLoaded = true
                };
            }
            Messenger.Default.Send(new BasicDataManagerRefreshedMessage());

            RaisePropertyChanged(nameof(MenuVisibility));
        }

        #region Fields

        private MyViewModelBase _SelectedViewmodel;

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
            get => _SelectedViewmodel;

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
            get => _Visibility;

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

        private BasicDataManager BasicDataManager;

        #endregion Properties

        public async Task LoadAsync(GenericRepository startingRepository)
        {
            try
            {
                if (SelectedViewmodel != null)
                {
                    SelectedViewmodel.IsLoaded = false;
                }

                StartingRepository = startingRepository;
                BasicDataManager = new BasicDataManager(StartingRepository);
                //#if DEBUG
                //                StaticResources.User = new User { Name = "admin", Id = 3, Level = 0 };
                //                RaisePropertyChanged(nameof(MenuVisibility));
                //#endif
                if (StaticResources.User == null)
                    SelectedViewmodel = new LoginViewModel(BasicDataManager);//TODO
                else
                {
                    SelectedViewmodel = new MainUserControl_ViewModel(BasicDataManager);//TODO
                }
                await BasicDataManager.LoadAsync();
                SelectedViewmodel.IsLoaded = true;
                CommandManager.InvalidateRequerySuggested();

            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }
    }
}